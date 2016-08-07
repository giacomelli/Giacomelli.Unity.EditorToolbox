using UnityEditor;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using Giacomelli.Unity.Metadata;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Giacomelli.Unity.EditorToolbox
{
	public class MissingScriptResolverWindow : WindowBase
	{
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
		}

		void SearchMissingScripts()
		{
			ResetLog();
			var scripts = ScriptMetadataService.GetAllScripts();
			var prefabs = PrefabMetadataService.GetAllPrefabs();

			foreach (var prefab in prefabs)
			{
				Log("Prefab: {0}", prefab.Name);
				Log("\t{0} missing scripts.", prefab.MonoBehaviours.Count);
				var missingMonoBehaviours = prefab.GetMissingMonoBehaviours();

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
				var missingMaterials = prefab.GetMissingMaterials();
				Log("\t{0} missing materials.", missingMaterials.Count);

				foreach (var m in missingMaterials)
				{
					Log("\t\tMaterial: {0} = {1}", m.FileId, m.FullName);
				}
			}
		}
		#endregion
	}

}

