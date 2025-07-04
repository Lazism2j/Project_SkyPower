using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IO;

[System.Serializable]
public partial class GameData : SaveData
{
    public string playerName;

    public CharacterInventory characterInventory;
    public StageInfo[] stageInfo;
    internal object equipmentInventory;

    public bool isEmpty => string.IsNullOrEmpty(playerName);

    public GameData()
    {
        // Initialize character inventory
        characterInventory = new CharacterInventory();
    }

}

// ����ȭ�� ����� �ҷ��� �� ����. json�� ����ȭ �������̶� �׷���.
[System.Serializable]
public struct StageInfo
{
    public int world;
    public int stage;
    public int score;
    public bool unlock;
    public bool isClear;
}