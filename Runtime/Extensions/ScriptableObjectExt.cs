#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HeroLib
{
    public static partial class ScriptableObjectExt {

	    public static void SafeSave(this ScriptableObject thisSO)
        {
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(thisSO);
            AssetDatabase.SaveAssets();
            Debug.Log("Saved " + thisSO.name + "asset file", thisSO);
        }
    }
}
#endif
