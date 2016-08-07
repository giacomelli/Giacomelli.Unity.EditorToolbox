using System;
using System.Collections.Generic;
using Giacomelli.Unity.Metadata;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Giacomelli.Unity.EditorToolbox
{
	public static class PrefabMetadataExtensions
	{
		public static IList<MonoBehaviourMetadata> GetMissingMonoBehaviours(this PrefabMetadata prefab)
		{
			var prefabInstance = AssetDatabase.LoadAssetAtPath<GameObject>(prefab.Path);

			if (prefabInstance == null)
			{
				throw new InvalidOperationException(
					"Cannot load the prefab {0} in the path {1}.".With(prefab.Name, prefab.Path));
			}

			var result = new List<MonoBehaviourMetadata>();

			foreach (var m in prefab.MonoBehaviours)
			{
				if (!prefabInstance.HasComponent (m.Script.FullName) )
				{
					result.Add(m);
				}
			}

			return result;
		}

		public static IList<MaterialMetadata> GetMissingMaterials(this PrefabMetadata prefab)
		{
			var prefabInstance = AssetDatabase.LoadAssetAtPath<GameObject>(prefab.Path);

			if (prefabInstance == null)
			{
				throw new InvalidOperationException(
					"Cannot load the prefab {0} in the path {1}.".With(prefab.Name, prefab.Path));
			}

			var result = new List<MaterialMetadata>();
			var instanceMaterials = prefabInstance.GetComponents<Renderer>().SelectMany(r => r.sharedMaterials);

			foreach (var m in prefab.Materials)
			{
				if (!instanceMaterials.All(i => i.name.Equals(m.Name)))
				{
					result.Add(m);
				}
			}

			return result;
		}

		public static void FillScriptsNames(this IEnumerable<PrefabMetadata> prefabs, IEnumerable<ScriptMetadata> allScripts)
		{
			foreach (var prefab in prefabs)
			{
				prefab.FillScriptsNames(allScripts);
			}
		}

		public static void FillScriptsNames(this PrefabMetadata prefab, IEnumerable<ScriptMetadata> allScripts)
		{
			foreach (var m in prefab.MonoBehaviours)
			{
				if (String.IsNullOrEmpty(m.Script.FullName))
				{
					var availableScript = allScripts.FirstOrDefault(s => s.FileId == m.Script.FileId);

					if (availableScript != null)
					{
						m.Script.FullName = availableScript.FullName;
					}
				}
			}
		}
	}
}

