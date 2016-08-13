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

        /// <summary>
        /// Draws the window's GUI.
        /// </summary>
        private void OnGUI()
        {
            if (GUILayout.Button("Search"))
            {
                SearchMissingScripts();
            }

            CreateLogView(minSize.y - 10);

            if (m_prefabsWithMissingScripts.Count > 0)
            {
                var style = new GUIStyle(GUI.skin.button);
                style.normal.textColor = Color.red;

                if (GUILayout.Button("Fix all", style))
                {
                    FixMissingMonobehaviours();
                }
            }
        }

        private void SearchMissingScripts()
        {
            ResetLog();
            m_prefabsWithMissingScripts = new Dictionary<PrefabMetadata, IEnumerable<MonoBehaviourMetadata>>();
            var scripts = MetadataBootstrap.ScriptMetadataService.GetAllScripts();
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

                // Materials.
                var missingMaterials = prefab.GetMissingMaterials(assetRepository);
                Log("\t{0} missing materials.", missingMaterials.Count());

                foreach (var m in missingMaterials)
                {
                    Log("\t\tMaterial: {0} = {1}", m.FileId, m.FullName);
                }
            }
        }

        private void FixMissingMonobehaviours()
        {
            var assetRepository = MetadataBootstrap.AssetRepository;
            var prefabService = MetadataBootstrap.PrefabMetadataService;
            var log = MetadataBootstrap.Log;

            foreach (var p in m_prefabsWithMissingScripts)
            {
                log.Debug("Fixing missing scripts for '{0}'...", p.Key.Name);
                prefabService.FixMissingMonobehaviours(p.Key, p.Value);
                break;
            }

            log.Debug("Done.");
        }
        #endregion
    }
}