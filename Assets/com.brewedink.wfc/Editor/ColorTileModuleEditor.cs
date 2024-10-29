using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using BrewedInk.WFC;


[CustomEditor(typeof(ColorTileWCFConfig))]
public class ColorTileEditor : Editor
{
    
    
    int currentPickerWindow;
    private Vector2 _scrollPosition;
    
    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI(); TODO: Basic props...
        
        

        var config = target as ColorTileWCFConfig;
        if (config == null) return;

        var totalVerticalRect = EditorGUILayout.BeginVertical(GUILayout.Height(300));
        
        EditorGUI.BeginChangeCheck();
        var nextuseSeed = EditorGUILayout.Toggle("Use Seed", config.useSeed);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Change use seed");
            config.useSeed = nextuseSeed;
            EditorUtility.SetDirty(config);
        }
        EditorGUI.BeginChangeCheck();
        var nextSeed = EditorGUILayout.IntField("Seed", config.seed);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Change seed");
            config.seed = nextSeed;
            EditorUtility.SetDirty(config);
        }

        
        EditorGUI.BeginChangeCheck();
        var nextWidth = EditorGUILayout.IntField("Width", config.Size.x);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Change width");
            config.Size.x = nextWidth;
            EditorUtility.SetDirty(config);
        }
        
        EditorGUI.BeginChangeCheck();
        var nextHeight = EditorGUILayout.IntField("Height", config.Size.y);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Change height");
            config.Size.y = nextHeight;
            EditorUtility.SetDirty(config);
        }
        
        EditorGUI.BeginChangeCheck();
        var nextSampleCount = EditorGUILayout.IntSlider("Sample Count", config.SampleCount, 1, 5);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Change sample count");
            config.SampleCount = nextSampleCount;
            EditorUtility.SetDirty(config);
        }
        
        EditorGUI.BeginChangeCheck();
        var nextSampleCompression = EditorGUILayout.Slider("Sample Spacing Ratio", config.SampleCompressionRatio, .25f, 2);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Change sample compression ratio");
            config.SampleCompressionRatio = nextSampleCompression;
            EditorUtility.SetDirty(config);
        }
        
        EditorGUI.BeginChangeCheck();
        var nextSampleCompressionOffset = EditorGUILayout.Slider("Sample Spacing Offset", config.SampleCompressionOffset, -1, 1);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Change sample offset");
            config.SampleCompressionOffset = nextSampleCompressionOffset;
            EditorUtility.SetDirty(config);
        }
        
        EditorGUI.BeginChangeCheck();
        var nextTolerance = EditorGUILayout.FloatField("Sample Tolerance", config.SampleTolerance);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Change tolerance");
            config.SampleTolerance = nextTolerance;
            EditorUtility.SetDirty(config);

        }

        
        EditorGUI.BeginChangeCheck();
        var nextLeftPad = EditorGUILayout.IntField("Sample Left Padding", config.LeftSamplePad);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Change left padding");
            config.LeftSamplePad = nextLeftPad;
            EditorUtility.SetDirty(config);

        }
        
        EditorGUI.BeginChangeCheck();
        var nextRight = EditorGUILayout.IntField("Sample Right Padding", config.RightSamplePad);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Change right padding");
            config.RightSamplePad = nextRight;
            EditorUtility.SetDirty(config);

        }
        
        EditorGUI.BeginChangeCheck();
        var nextTop = EditorGUILayout.IntField("Sample Top Padding", config.TopSamplePad);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Change top padding");
            config.TopSamplePad = nextTop;
            EditorUtility.SetDirty(config);

        }
        
        EditorGUI.BeginChangeCheck();
        var nextLow = EditorGUILayout.IntField("Sample Low Padding", config.LowSamplePad);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Change low");
            config.LowSamplePad = nextLow;
            EditorUtility.SetDirty(config);

        }
        
        var constraintGenerator = new EdgeColorConstraintGenerator();
        constraintGenerator.SampleCount = config.SampleCount;
        constraintGenerator.LeftSamplePad = config.LeftSamplePad;
        constraintGenerator.RightSamplePad = config.RightSamplePad;
        constraintGenerator.TopSamplePad = config.TopSamplePad;
        constraintGenerator.LowSamplePad = config.LowSamplePad;
        constraintGenerator.ColorTolerance = config.SampleTolerance;
        constraintGenerator.SampleCompressionRatio = config.SampleCompressionRatio;
        constraintGenerator.SampleCompressionOffset = config.SampleCompressionOffset;
        
        if (GUILayout.Button("Clear All Sprites"))
        {
            Undo.RecordObject(target, "Clear all sprites");
            config.moduleObjects.Clear();
            EditorUtility.SetDirty(config);

        }

        if (GUILayout.Button("Add from Sprite Sheet"))
        {
            Texture2D txt = null;
            //create a window picker control ID
            currentPickerWindow = EditorGUIUtility.GetControlID(FocusType.Passive) + 100;
            EditorGUIUtility.ShowObjectPicker<Texture2D>(txt, false, "", currentPickerWindow);

        }

        if (Event.current.commandName == "ObjectSelectorUpdated" &&
            EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
        {
            Undo.RecordObject(target, "Set sprite");

            var txt = EditorGUIUtility.GetObjectPickerObject() as Texture2D;
            Debug.Log("Texture " + txt.name);
            string spriteSheet = AssetDatabase.GetAssetPath( txt );
            Debug.Log("Loading sprite sheet at path " + spriteSheet);
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath( spriteSheet )
                .OfType<Sprite>().ToArray();
            Debug.Log("Found " + sprites.Length + " sprites");
            
            var newModules = sprites.Select(s => new ColorTileModuleObject{
                Module = new ColorTileModule(s)
            }).ToList();
            config.moduleObjects.AddRange(newModules);
            currentPickerWindow = -1;

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(config);
        }


        if (config.moduleObjects == null)
        {
            config.moduleObjects = new List<ColorTileModuleObject>();
            EditorUtility.SetDirty(config);
        }

        ColorTileModuleObject toDelete = null;

     
        //var verticalRect = EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(false));
        EditorGUILayout.EndVertical();

        var trueHeight = (Screen.height - 200);

        //var h = EditorGUIUtility.pixelsPerPoint * Screen.height;
        //Debug.Log(h);
        // _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.MaxHeight(h - totalVerticalRect.height*EditorGUIUtility.pixelsPerPoint));
        for (var i = 0 ; i < config.moduleObjects.Count; i ++)
        {
            var module = config.moduleObjects[i];
            if (string.IsNullOrEmpty(module.Module.Display))
            {
                module.Module.Display = module.Module.Sprite == null ? null : module.Module.Sprite.name;
            }
                        
            EditorGUILayout.BeginHorizontal();
            // GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(module.Module.Display);

            if (GUILayout.Button("Remove Module", GUILayout.Width(200)))
            {
                toDelete = module;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.BeginChangeCheck();
            var nextWeight = EditorGUILayout.FloatField("Weight", module.Module.Weight);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Change weight");
                module.Module.Weight = nextWeight;
                EditorUtility.SetDirty(target);
            }
            EditorGUI.BeginChangeCheck();
            var nextSprite = EditorGUILayout.ObjectField("Sprite", module.Module.Sprite, typeof(Sprite), false) as Sprite;
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Change sprite");
                module.Module.Sprite = nextSprite;
                module.Module.Display = module.Module.Sprite.name;
                EditorUtility.SetDirty(target);
            }

            
            Texture2D texture = AssetPreview.GetAssetPreview(module.Module.Sprite);

            var lastRect = GUILayoutUtility.GetLastRect();
            EditorGUILayout.Space(25);

            var previewRect = new Rect(lastRect.x + 25, lastRect.yMax + 25, 100, 100);

            var lastY = previewRect.yMin;
            var lastX = previewRect.xMin;
            if (texture != null)
            {
                EditorGUI.DrawPreviewTexture(previewRect, texture, null, ScaleMode.ScaleToFit);
            }

            EditorGUILayout.Space(25);

            
            EditorGUILayout.Space(100);

            if (module.Module.Sprite == null)
            {
                EditorGUI.LabelField(previewRect, "<no sprite>");
            }
            else
            {


                var leftSamples = constraintGenerator.GetLeftSamples(module.Module.Sprite);
                var rightSamples = constraintGenerator.GetRightSamples(module.Module.Sprite);
                var topSamples = constraintGenerator.GetTopSamples(module.Module.Sprite);
                var lowSamples = constraintGenerator.GetLowSamples(module.Module.Sprite);


                for (var j = 0; j < leftSamples.Length; j++)
                {
                    var leftRect = new Rect(lastRect.x, lastY, 20, 10);
                    var rightRect = new Rect(previewRect.xMax + 12, lastY, 20, 10);

                    var lowRect = new Rect(lastX, previewRect.yMin - 25, 15, 20);
                    var topRect = new Rect(lastX, previewRect.yMax + 5, 15, 20);

                    DrawHintRect(module.Module.Sprite, leftSamples[j].Position, previewRect);
                    DrawHintRect(module.Module.Sprite, rightSamples[j].Position, previewRect);
                    DrawHintRect(module.Module.Sprite, topSamples[j].Position, previewRect);
                    DrawHintRect(module.Module.Sprite, lowSamples[j].Position, previewRect);
                    EditorGUI.DrawRect(leftRect, leftSamples[j].Color);
                    EditorGUI.DrawRect(rightRect, rightSamples[j].Color);
                    EditorGUI.DrawRect(topRect, topSamples[j].Color);
                    EditorGUI.DrawRect(lowRect, lowSamples[j].Color);

                    lastY += 12;
                    lastX += 17;

                }
            }

            EditorGUILayout.Space(12);
            Rect rect = EditorGUILayout.GetControlRect(false, 1 );

            rect.height = 1;

            EditorGUI.DrawRect(rect, new Color ( 0.5f,0.5f,0.5f, 1 ) );
            EditorGUILayout.Space(12);

        }

       //  EditorGUILayout.EndScrollView();
       // EditorGUILayout.EndVertical();
        //EditorGUI.DrawRect(verticalRect, Color.red);
        
        if (toDelete != null)
        {
            Undo.RecordObject(target, "Remove module");
            config.moduleObjects.Remove(toDelete);
            toDelete = null;
            EditorUtility.SetDirty(target);
        }

        if (GUILayout.Button("Add new Module"))
        {
            Undo.RecordObject(target, "Add module");
            config.moduleObjects.Add(new ColorTileModuleObject
            {
                Module = new ColorTileModule()
            });
            EditorUtility.SetDirty(target);
        }
        
        
    }

    void DrawHintRect(Sprite sprite, Vector2Int position, Rect previewRect)
    {
        var yRatio = (position.y - sprite.textureRect.yMin) /
                     (sprite.textureRect.height);
        var xRatio = (position.x - sprite.textureRect.xMin) /
                     sprite.textureRect.width;

        var hintRect = new Rect(previewRect.x + (xRatio * 100), previewRect.y + ((1 - yRatio) * 100), 2, 2);

        EditorGUI.DrawRect(hintRect, Color.red);
    }
}

[CustomPropertyDrawer(typeof(ColorTileModuleObject))]
public class ColorTileModuleEditor : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 100;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        var moduleProp = property.FindPropertyRelative(nameof(ColorTileModuleObject.Module));
        var spriteProp = moduleProp.FindPropertyRelative(nameof(ColorTileModuleObject.Module.Sprite));
        var fieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.ObjectField(fieldRect, spriteProp);
        
        var previewRect = new Rect(position.x,fieldRect.yMax, fieldRect.width, 80);

        Texture2D texture = AssetPreview.GetAssetPreview(spriteProp.objectReferenceValue);
        
        
        
        //var texture = spriteProp.serializedObject as Sprite;
        EditorGUI.DrawPreviewTexture(previewRect, texture, null, ScaleMode.ScaleToFit);
        //EditorGUI.LabelField(position, "test");
    }
}
