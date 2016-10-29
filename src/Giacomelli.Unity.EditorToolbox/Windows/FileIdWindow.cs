#if DEBUG
using Giacomelli.Unity.Metadata.Infrastructure.Framework.Serialization;
using UnityEditor;
using UnityEngine;

namespace Giacomelli.Unity.EditorToolbox
{
	public class FileIdWindow : WindowBase
    {
        #region Fields
      	private string m_typeName = string.Empty;
		#endregion

        #region Constructors
        public FileIdWindow()
            : base("FileId", 400, 400)
        {
        }
        #endregion

        #region GUI
        [MenuItem("Giacomelli/FileId", priority = 2)]
        private static void Init()
        {
            var instance = GetWindow<FileIdWindow>();
            instance.ShowUtility();
        }

        protected override void PerformOnGUI()
        {
			m_typeName = GUILayout.TextField(m_typeName);

			if (!string.IsNullOrEmpty(m_typeName))
			{
				GUILayout.Label(FileIdUtil.FromType(m_typeName).ToString());
			}
        }
        #endregion
    }
}
#endif