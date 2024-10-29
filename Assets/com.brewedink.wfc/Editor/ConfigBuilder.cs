using System.IO;
using UnityEditor;
using UnityEngine;

namespace BrewedInk.EditorTools.WFC
{
    public class ConfigBuilder
    {
        private const string ConfigPath = "Assets/com.brewedink.wfc/Editor/ScriptTemplates/newConfig.cs.txt";
        
        [MenuItem(itemName:"WFC Configuration", menuItem = "Assets/Create/BrewedInk WFC/C# Config", priority = 100)]
        public static void Create()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(ConfigPath, "MyWFCConfig.cs");
            
        }
    }
}