﻿using UnityEditor;
using UnityEngine;

namespace Assets.TutorialInfo
{
    public class NewEditor : ScriptableObject
    {
        [MenuItem("Tools/MyTool/Do It in C#")]
        static void DoIt()
        {
            EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
        }
    }
}