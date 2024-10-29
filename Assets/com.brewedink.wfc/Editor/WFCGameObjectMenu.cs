using UnityEditor;
using UnityEngine;

namespace BrewedInk.EditorTools.WFC
{
    public static class WFCGameObjectMenu
    {
        [MenuItem("GameObject/Brewed Ink WFC/2D Grid Renderer", priority = 10)]
        public static void AddWFC()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/com.brewedink.wfc/Runtime/WFCBoard.prefab");
            
            var obj = PrefabUtility.InstantiatePrefab(prefab);
            Selection.SetActiveObjectWithContext(obj, obj);
        }
    }
}