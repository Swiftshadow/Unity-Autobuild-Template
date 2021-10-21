using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace DisasterGolf.Editor
{
    /// <summary>
    /// GUI to test the autobuild functions
    /// </summary>
    public class BuildGUI : OdinEditorWindow
    {
        /// <summary>
        /// Used to show the Build GUI
        /// </summary>
        [MenuItem("Build/BuildGUI")]
        private static void OpenWindow()
        {
            GetWindow<BuildGUI>().Show();
        }

        /// <summary>
        /// Builds the game for windows
        /// </summary>
        [Button]
        public static void BuildWindows()
        {
            BuildScript.BuildWindows();
        }

        /// <summary>
        /// Builds the game for Mac
        /// </summary>
        [Button]
        public static void BuildMac()
        {
            BuildScript.BuildMac();
        }
    }
}