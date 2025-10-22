using UnityEngine;
using UnityEditor;

public class LegacySetter
{
    [MenuItem("Tools/Set Selected Clips to Legacy")]
    static void SetLegacy()
    {
        foreach (var obj in Selection.objects)
        {
            if (obj is AnimationClip clip)
            {
                clip.legacy = true;
                EditorUtility.SetDirty(clip);
                Debug.Log($"{clip.name} marked as Legacy");
            }
        }
        AssetDatabase.SaveAssets();
    }
}
