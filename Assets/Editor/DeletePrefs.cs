using UnityEngine;
using UnityEditor;

public class DeletePrefs : ScriptableObject
{

    [MenuItem("Assets/PlayerPrefs/DeleteAll")]
    static void ClearPlayerPrefs()
    {
        if (EditorUtility.DisplayDialog("Delete all player preferences.",
                                       "Are you sure you want to delete all the player preferences? " +
                                       "This action cannot be undone.", "Yes", "No"))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
