using System;
using System.Collections.Generic;
using System.IO;
using Giacomelli.Unity.Metadata;
using UnityEngine;

namespace Giacomelli.Unity.EditorToolbox
{
	// TODO: Maybe this code should be move do Giacomelli.Unity.Metadata.
	public static class PrefabMetadataService
	{
		public static IList<PrefabMetadata> GetAllPrefabs()
		{
			var prefabs = new List<PrefabMetadata>();
			var rootPath = Application.dataPath + "/";
			var prefabFiles = Directory.GetFiles(rootPath, "*.prefab", SearchOption.AllDirectories);
			var prefabReader = new YamlPrefabMetadataReader();
			var scripts = ScriptMetadataService.GetAllScripts();

			foreach (var path in prefabFiles)
			{
				var prefab = prefabReader.ReadFromFile(path);
				prefab.Name = Path.GetFileNameWithoutExtension(path);
				prefab.Path = "Assets/" + path.Replace(rootPath, string.Empty);
				//prefab.FillScriptsNames(scripts);
				prefabs.Add(prefab);
			}

			return prefabs;
		}
	}
}