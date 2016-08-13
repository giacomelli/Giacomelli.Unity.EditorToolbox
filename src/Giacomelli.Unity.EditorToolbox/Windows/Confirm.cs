using System;
using UnityEditor;
using UnityEngine;

namespace Giacomelli.Unity.EditorToolbox
{
	public class Confirm
	{
		private string m_message;
		private Action m_yesCallback;
		private Action m_noCallback;

		public Confirm(string message, Action yesCallback, Action noCallback = null)
		{
			if (String.IsNullOrEmpty(message))
			{
				throw new ArgumentNullException("message");
			}

			if (yesCallback == null)
			{
				throw new ArgumentNullException("yesCallback");
			}

			m_message = message;
			m_yesCallback = yesCallback;

			if (noCallback == null)
			{
				m_noCallback = () => { };
			}
			else
			{
				m_noCallback = noCallback;
			}
		}

		public void OnGUI()
		{
			GUILayout.BeginVertical(GUILayout.ExpandHeight(true));
			GUILayout.TextArea(m_message, GUILayout.ExpandHeight(true));

			if (GUILayout.Button("Yes"))
			{
				m_yesCallback();
			}

			if (GUILayout.Button("No"))
			{
				m_noCallback();
			}

			GUILayout.EndVertical();
		}
	}
}