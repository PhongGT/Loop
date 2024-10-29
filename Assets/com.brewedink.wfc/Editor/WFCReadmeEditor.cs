using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace BrewedInk.EditorTools.WFC
{
    [CustomEditor(typeof(WFCReadme))]
    [InitializeOnLoad]
    public class WFCReadmeEditor : Editor
    {
        static string kShowedReadmeSessionStateName = "BrewedInk.WFC.showedReadme";
	
        static float kSpace = 16f;
	
        static WFCReadmeEditor()
        {
            EditorApplication.delayCall += SelectReadmeAutomatically;
        }
	
        static void SelectReadmeAutomatically()
        {
            if (!SessionState.GetBool(kShowedReadmeSessionStateName, false ))
            {
                var readme = SelectReadme();
                SessionState.SetBool(kShowedReadmeSessionStateName, true);
            } 
        }
        
    [MenuItem("Help/Wave Fuction Collapser")]
	static WFCReadme SelectReadme() 
	{
		var ids = AssetDatabase.FindAssets("Readme t:WFCReadme");
		if (ids.Length == 1)
		{
			var readmeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));
			
			Selection.objects = new UnityEngine.Object[]{readmeObject};
			
			return (WFCReadme)readmeObject;
		}
		else
		{
			Debug.Log("Couldn't find a readme");
			return null;
		}
	}
	
	protected override void OnHeaderGUI()
	{
		var readme = (WFCReadme)target;
		Init();
		
		var iconWidth = Mathf.Min(EditorGUIUtility.currentViewWidth/3f - 20f, readme.iconMaxWidth);
		
		GUILayout.BeginHorizontal("In BigTitle");
		{
			GUILayout.Label(readme.icon, GUILayout.Width(iconWidth), GUILayout.Height(iconWidth));
			GUILayout.Label(readme.title, TitleStyle);
		}
		GUILayout.EndHorizontal();
	}

	void DrawLine()
	{
			
		EditorGUILayout.Space(12);
		Rect rect = EditorGUILayout.GetControlRect(false, 1 );
		rect.height = 1;
		EditorGUI.DrawRect(rect, new Color ( 0.5f,0.5f,0.5f, 1 ) );
		EditorGUILayout.Space(12);

	}
	
	public override void OnInspectorGUI()
	{
		var readme = (WFCReadme)target;
		Init();
		
		GUILayout.Label(readme.gettingStartedSection.heading, HeadingStyle);
		GUILayout.Label(readme.gettingStartedSection.text, BodyStyle);

		var buttonsPerRow = Mathf.CeilToInt(EditorGUIUtility.currentViewWidth / 256);
		if (buttonsPerRow % 2 == 1)
		{
			buttonsPerRow--;
		}
		var rows = Mathf.CeilToInt(readme.gettingStartedSection.examples.Count / buttonsPerRow);
		if (rows < 1) rows = 1;
		
		
		
		var rendered = 0;
		while (rendered <= readme.gettingStartedSection.examples.Count)
		{
			var start = rendered;
			var end = start + buttonsPerRow;
			rendered = end;
			if (end >= readme.gettingStartedSection.examples.Count)
			{
				end = readme.gettingStartedSection.examples.Count ;
			}
			
			GUILayout.Space(15);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			for (var i = start; i < end; i++)
			{
				var sceneButton = readme.gettingStartedSection.examples[i];
				GUILayout.Label(sceneButton.title, GUILayout.Width(128));
				GUILayout.Space(10);
			}

			GUILayout.FlexibleSpace();

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			for (var i = start; i < end; i++)
			{
				var sceneButton = readme.gettingStartedSection.examples[i];

				if (GUILayout.Button(new GUIContent(sceneButton.icon), GUILayout.Width(128), GUILayout.Height(128)))
				{
					EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
					EditorSceneManager.OpenScene(sceneButton.scenePath);
				}

				GUILayout.Space(10);

			}

			GUILayout.FlexibleSpace();

			GUILayout.EndHorizontal();
		}

		DrawLine();
		
		GUILayout.Label(readme.docSection.heading, HeadingStyle);
		GUILayout.Label(readme.docSection.text, BodyStyle);
		
		foreach (var section in readme.docSection.links)
		{
			if (!string.IsNullOrEmpty(section.linkText))
			{
				GUILayout.Space(kSpace / 2);
				if (LinkLabel(new GUIContent(section.linkText)))
				{
					Application.OpenURL(section.url);
				}
				GUILayout.Label(section.about, BodyStyle);

			}
			
			GUILayout.Space(kSpace);
		}
		DrawLine();

	}
	
	
	bool m_Initialized;
	
	GUIStyle LinkStyle { get { return m_LinkStyle; } }
	[SerializeField] GUIStyle m_LinkStyle;
	
	GUIStyle TitleStyle { get { return m_TitleStyle; } }
	[SerializeField] GUIStyle m_TitleStyle;
	
	GUIStyle HeadingStyle { get { return m_HeadingStyle; } }
	[SerializeField] GUIStyle m_HeadingStyle;
	
	GUIStyle BodyStyle { get { return m_BodyStyle; } }
	[SerializeField] GUIStyle m_BodyStyle;
	
	void Init()
	{
		if (m_Initialized)
			return;
		m_BodyStyle = new GUIStyle(EditorStyles.label);
		m_BodyStyle.wordWrap = true;
		m_BodyStyle.fontSize = 14;
		
		m_TitleStyle = new GUIStyle(m_BodyStyle);
		m_TitleStyle.fontSize = 26;

		m_HeadingStyle = new GUIStyle(m_BodyStyle);
		m_HeadingStyle.fontSize = 18;
		m_HeadingStyle.fontStyle = FontStyle.Bold;
		
		m_LinkStyle = new GUIStyle(m_BodyStyle);
		// Match selection color which works nicely for both light and dark skins
		m_LinkStyle.normal.textColor = new Color (0x00/255f, 0x78/255f, 0xDA/255f, 1f);
		m_LinkStyle.stretchWidth = false;
		
		m_Initialized = true;
	}
	
	bool LinkLabel (GUIContent label, params GUILayoutOption[] options)
	{
		var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

		Handles.BeginGUI ();
		Handles.color = LinkStyle.normal.textColor;
		Handles.DrawLine (new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
		Handles.color = Color.white;
		Handles.EndGUI ();

		EditorGUIUtility.AddCursorRect (position, MouseCursor.Link);

		return GUI.Button (position, label, LinkStyle);
	}
    }
}