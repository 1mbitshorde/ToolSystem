using UnityEditor;
using UnityEngine;

namespace OneM.UtilitySystem.Editor
{
    /// <summary>
    /// Rename Asset tool.
    /// </summary>
    public class RenameAssets : EditorWindow
    {
        private string assetPrefix;
        private string assetSuffix;
        private string replaceTextOrigin;
        private bool renameFullName;
        private bool replaceTextToggle;

        [MenuItem("Tools/Rename")]
        private static void ShowWindow() => GetWindow<RenameAssets>("Rename Assets");

        private void OnGUI()
        {
            GUILayout.Label("Rename Selected Assets", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Options of this tool \n" +
                                    "- Rename full name of the selected assets. This will add a counter at the " +
                                    "beginning; \n" +
                                    "- Just add a prefix in front of the asset name; \n" +
                                    "- And/or replace a text inside the asset name." +
                                    "", MessageType.Info);
            EditorGUILayout.Space();

            renameFullName = EditorGUILayout.Toggle("Replace full text:", renameFullName);
            assetPrefix = EditorGUILayout.TextField("Prefix", assetPrefix);
            assetSuffix = EditorGUILayout.TextField("Suffix", assetSuffix);
            if (GUILayout.Button("Add Prefix or Replace Full Text"))
                RenameSelectedAssets();
            EditorGUILayout.Space();
            EditorGUILayout.Separator();
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Replace the given name inside the asset name by the Prefix." +
                                    "", MessageType.Info);
            replaceTextToggle = EditorGUILayout.Toggle("Replace only the given text:", replaceTextToggle);
            replaceTextOrigin = EditorGUILayout.TextField("Text to replace by Prefix:", replaceTextOrigin);
            if (GUILayout.Button("Replace Text"))
                ReplaceTextInSelectedAssets();

            EditorGUILayout.Space();
            EditorGUILayout.Separator();
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Remove the given name inside the asset name by the Prefix." +
                                    "", MessageType.Info);
            replaceTextOrigin = EditorGUILayout.TextField("Text to remove:", replaceTextOrigin);
            if (GUILayout.Button("Remove Text"))
                RemoveTextInSelectedAssets();
        }

        private void RenameSelectedAssets()
        {
            int counter = 0;
            foreach (Object obj in Selection.objects)
            {
                string assetPath = AssetDatabase.GetAssetPath(obj);
                string newName = "";
                if (renameFullName)
                    newName = obj.name + "_" + counter;
                else
                    newName = assetPrefix + obj.name + assetSuffix;
                AssetDatabase.RenameAsset(assetPath, newName);
                counter++;
            }
        }

        private void ReplaceTextInSelectedAssets()
        {
            foreach (Object obj in Selection.objects)
            {
                string assetPath = AssetDatabase.GetAssetPath(obj);
                string assetName = obj.name;

                // Perform the text replacement
                string newName = assetName.Replace(replaceTextOrigin, assetPrefix);

                // Rename the asset
                AssetDatabase.RenameAsset(assetPath, newName);
            }
        }

        private void RemoveTextInSelectedAssets()
        {
            foreach (Object obj in Selection.objects)
            {
                string assetPath = AssetDatabase.GetAssetPath(obj);
                string assetName = obj.name;

                // Perform the text removal
                string newName = assetName.Replace(replaceTextOrigin, "");

                // Rename the asset
                AssetDatabase.RenameAsset(assetPath, newName);
            }
        }
    }
}