using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BrewedInk.EditorTools.WFC
{
    public class WFCReadme : ScriptableObject
    {
        public Texture2D icon;
        public float iconMaxWidth = 128f;
        public string title;
        public GettingStartedSection gettingStartedSection;
        public DocumentationSection docSection;
        
        [Serializable]
        public class DocumentationSection
        {
            public string heading;
            public string text;
            public List<DocLink> links;
        }

        [Serializable]
        public class DocLink
        {
            public string linkText;
            public string url;
            public string about;
        }
        
        [Serializable]
        public class GettingStartedSection
        {
            public string heading;
            public string text;
            public List<SceneButtonData> examples;
        }

        [Serializable]
        public class SceneButtonData
        {
            public Texture2D icon;
            public string title;
            public string scenePath;
        }
    }
}