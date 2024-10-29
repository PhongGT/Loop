using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BrewedInk.EditorTools.WFC
{
    [CustomEditor(typeof(SpriteConfig))]
    public class SpriteConfigEditor : Editor
    {
        private Vector2 scrollPosition;
        
        public override void OnInspectorGUI()
        {
            var config = target as SpriteConfig;
            if (config == null) return;

            Field( "Use Seed", lbl => EditorGUILayout.Toggle(lbl, config.useSeed), v => config.useSeed = v);
            Field( "Seed", lbl => EditorGUILayout.IntField(lbl, config.seed), v => config.seed = v);
            Field( "Width", lbl => EditorGUILayout.IntField(lbl, config.Width), v => config.Width = v);
            Field( "Height", lbl => EditorGUILayout.IntField(lbl, config.Height), v => config.Height = v);

            var listProp = serializedObject.FindProperty(nameof(config.moduleObjects));
            listProp.isExpanded = EditorGUILayout.Foldout(listProp.isExpanded, "Modules");
            if (listProp.isExpanded)
            {
                try
                {
                    EditorGUI.indentLevel++;
                    var rect = GUILayoutUtility.GetLastRect();

                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

                    SpriteConfigModuleObject toDelete = null;
                    //foreach (var moduleObject in config.moduleObjects)
                    if (config.moduleObjects == null)
                    {
                        config.moduleObjects = new List<SpriteConfigModuleObject>();
                        EditorUtility.SetDirty(config);
                    }
                    for (var i = 0 ; i < config.moduleObjects.Count; i ++)
                    {
                        var moduleObject = config.moduleObjects[i];
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Delete Module"))
                        {
                            toDelete = moduleObject;
                        }
                        EditorGUILayout.EndHorizontal();
                        
                        Field("Name", lbl => EditorGUILayout.TextField(lbl, moduleObject.Module.Display), v => moduleObject.Module.Display = v);
                        Field("Weight", lbl => EditorGUILayout.FloatField(lbl, moduleObject.Module.Weight), v => moduleObject.Module.Weight = v);
                        
                        EditorGUILayout.Space(25);
                        rect = GUILayoutUtility.GetLastRect();


                        EditorGUILayout.BeginHorizontal();
                        GUILayout.FlexibleSpace();
                        EditorGUI.BeginChangeCheck();
                        var nextSprite = EditorGUILayout.ObjectField("", moduleObject.Module.sprite, typeof(Sprite), false, GUILayout.Width(80));
                        var spriteRect = GUILayoutUtility.GetLastRect();
                        if (EditorGUI.EndChangeCheck())
                        {
                            Undo.RecordObject(target, "Change sprite");
                            if (string.IsNullOrEmpty(moduleObject.Module.Display))
                            {
                                moduleObject.Module.Display = nextSprite.name;
                            }
                            moduleObject.Module.sprite = nextSprite as Sprite;
                            EditorUtility.SetDirty(target);
                        }
                        GUILayout.FlexibleSpace();
                        
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space(25);

                        var topSocketRect = new Rect(spriteRect.center.x - 45, spriteRect.yMin - 25, 90, EditorGUIUtility.singleLineHeight);
                        var lowSocketRect = new Rect(spriteRect.center.x - 45, spriteRect.yMax + 5, 90, EditorGUIUtility.singleLineHeight);
                        var leftSocketRect = new Rect(spriteRect.xMin - 100, spriteRect.center.y - 10, 90, EditorGUIUtility.singleLineHeight);
                        var rightSocketRect = new Rect(spriteRect.xMax + 10, spriteRect.center.y - 10, 90, EditorGUIUtility.singleLineHeight);
                        Field("left socket", lbl => EditorGUI.IntField(leftSocketRect, moduleObject.Module.leftSocket), v => moduleObject.Module.leftSocket = v);
                        Field("right socket", lbl => EditorGUI.IntField(rightSocketRect, moduleObject.Module.rightSocket), v => moduleObject.Module.rightSocket = v);
                        Field("top socket", lbl => EditorGUI.IntField(topSocketRect, moduleObject.Module.topSocket), v => moduleObject.Module.topSocket = v);
                        Field("low socket", lbl => EditorGUI.IntField(lowSocketRect, moduleObject.Module.lowSocket), v => moduleObject.Module.lowSocket = v);
                    }

                    if (toDelete != null)
                    {
                        Undo.RecordObject(target, "Remove module");
                        config.moduleObjects.Remove(toDelete);
                        EditorUtility.SetDirty(target);

                    }

                    if (GUILayout.Button("Add Module"))
                    {
                        Undo.RecordObject(target, "Add module");
                        config.moduleObjects.Add(new SpriteConfigModuleObject
                        {
                            Module = new SpriteConfigModule()
                        });
                        EditorUtility.SetDirty(target);

                    }
                }
                finally
                {
                    EditorGUILayout.EndScrollView();
                    EditorGUI.indentLevel--;
                }
            }
            
        }

        void Field<T>(string label, Func<string, T> render, Action<T> setter)
        {
            EditorGUI.BeginChangeCheck();
            var nextUseSeed = render(label);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, $"Change {label}");
                setter(nextUseSeed);
                EditorUtility.SetDirty(target);

            }
        }
        
    }
}