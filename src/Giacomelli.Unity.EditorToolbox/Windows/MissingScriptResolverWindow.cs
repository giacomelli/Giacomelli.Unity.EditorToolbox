using System;
using System.Collections.Generic;
using System.Linq;
using Giacomelli.Unity.Metadata.Domain;
using Giacomelli.Unity.Metadata.Infrastructure.Bootstrap;
using UnityEditor;
using UnityEngine;

namespace Giacomelli.Unity.EditorToolbox
{
    public class MissingScriptResolverWindow : WindowBase
    {
        #region Fields
        private Dictionary<PrefabMetadata, IEnumerable<MonoBehaviourMetadata>> m_prefabsWithMissingScripts = new Dictionary<PrefabMetadata, IEnumerable<MonoBehaviourMetadata>>();
		private int m_totalMissingScriptsPrefabs;
		private int m_totalMissingScripts;
		#endregion

        #region Constructors
        public MissingScriptResolverWindow()
            : base("Missing script", 400, 400)
        {
        }
        #endregion

        #region GUI
        [MenuItem("Giacomelli/Missing script resolver")]
        private static void Init()
        {
            var instance = GetWindow<MissingScriptResolverWindow>();
            instance.ShowUtility();
        }

        protected override void PerformOnGUI()
        {
            if (GUILayout.Button("Search"))
            {
                SearchMissingScripts();
            }

            CreateLogView(minSize.y - 10);

            if (m_totalMissingScripts > 0)
            {
                var style = new GUIStyle(GUI.skin.button);
                style.normal.textColor = Color.red;

				if (GUILayout.Button("Fix {0} missing scripts in {1} prefabs".With(m_totalMissingScripts, m_totalMissingScriptsPrefabs), style))
				{
					Confirm(
						"Do you really want to fix all missing scripts?\n\nRemember to make a backup before.",
						FixMissingMonobehaviours);
				}               
            }
        }

        private void SearchMissingScripts()
        {
			AssetDatabase.Refresh();
            ResetLog();
            m_prefabsWithMissingScripts = new Dictionary<PrefabMetadata, IEnumerable<MonoBehaviourMetadata>>();
            var scripts = MetadataBootstrap.ScriptMetadataService.GetScripts();
            var prefabs = MetadataBootstrap.PrefabMetadataService.GetPrefabs();
            var typeService = MetadataBootstrap.TypeService;
            var assetRepository = MetadataBootstrap.AssetRepository;

		    foreach (var prefab in prefabs)
            {
                var missingMonoBehaviours = prefab.GetMissingMonoBehaviours(assetRepository, typeService).ToArray();

                if (missingMonoBehaviours.Length == 0)
                {
			        continue;
                }

                Log("Prefab: {0}", prefab.Name);
                Log("\t{0} missing scripts.", missingMonoBehaviours.Length);

                m_prefabsWithMissingScripts.Add(prefab, missingMonoBehaviours);

                foreach (var m in missingMonoBehaviours)
                {
                    var fileId = m.Script.FileId;
                    var scriptName = "NOT FOUND";
                    var compatibleScripts = scripts.Where(s => s.FileId == fileId).ToArray();

                    if (compatibleScripts.Length > 0)
                    {
                        scriptName = String.Join(", ", compatibleScripts.Select(s => s.Name).ToArray());
                    }

                    Log("\t\tScript: {0}", scriptName);
                }
            }

			CalculateMissing();

			if (m_totalMissingScripts == 0)
			{
				Log("No missing scripts.");
			}
        }

        private void FixMissingMonobehaviours()
        {
			// Unselect any active object, because in Unity 5.3.5f1 it breaks when any changes happens 
			// on a selected prefab outside of editor.
			Selection.activeObject = null;

			ResetLog();
            var assetRepository = MetadataBootstrap.AssetRepository;
            var prefabService = MetadataBootstrap.PrefabMetadataService;
            var log = MetadataBootstrap.Log;
			var prefabNumber = 0;

            foreach (var p in m_prefabsWithMissingScripts)
            {
				Log("{0}) Fixing {1} missing scripts in '{2}' prefab.", ++prefabNumber, p.Value.Count(), p.Key.Name);
                prefabService.FixMissingMonobehaviours(p.Key, p.Value);
            }

			m_prefabsWithMissingScripts.Clear();
			CalculateMissing();

			Log("Done.");
			AssetDatabase.Refresh();
        }

		private void CalculateMissing()
		{
			m_totalMissingScriptsPrefabs = m_prefabsWithMissingScripts.Count;
			m_totalMissingScripts = m_prefabsWithMissingScripts.Sum(p => p.Value.Count());
		}
        #endregion
    }
}