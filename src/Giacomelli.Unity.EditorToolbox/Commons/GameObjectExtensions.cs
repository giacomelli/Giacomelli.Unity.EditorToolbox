using System;
using Giacomelli.Unity.EditorToolbox;
using Giacomelli.Unity.Metadata;
using UnityEngine;

public static class GameObjectExtensions
{
	public static bool HasComponent(this GameObject go, string typeName)
	{
		var type = TypesHelper.GetType(typeName);
		return go.GetComponentInChildren(type) != null;
	}
}