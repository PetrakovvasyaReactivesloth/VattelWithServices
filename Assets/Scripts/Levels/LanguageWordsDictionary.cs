using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LanguageWordsDictionary")]
public class LanguageWordsDictionary : ScriptableObject
{
    [System.Serializable]
    public class Difficulty
    {
        public string title;
        public string leaderBoardsTableID;
        public List<LevelScriptableObj> _levels = new List<LevelScriptableObj>();
    }

    public string _title;
    public List<Difficulty> _difficultyLevels = new List<Difficulty>();
}