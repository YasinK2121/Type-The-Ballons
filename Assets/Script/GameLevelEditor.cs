using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Editors/GameLevelEditor")]
[SerializeField]
public class GameLevelEditor : ScriptableObject
{
    [Header("WORD")]
    public List<Difficulty> difficulty;
    [Header("LEVEL")]
    public List<Level> levelData;
}

[System.Serializable]
public class Difficulty
{
    public string WhichDifficulty;
    public float speed;
    public List<Words> Words;
}

[System.Serializable]
public class Level
{
    public string LevelName;
    public List<LevelWords> levelWords;
}

[System.Serializable]
public class LevelWords
{
    public ItemCharacteristic item;
    public int howManyWords;
}

[System.Serializable]
public class Words
{
    public string word;
}

public enum ItemCharacteristic
{
    Difficulty1,
    Difficulty2,
    Difficulty3,
    Difficulty4,
    Difficulty5,
    Difficulty6,
    Difficulty7,
    Difficulty8,
    Difficulty9,
    Difficulty10,
};
