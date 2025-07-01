using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IO;

public partial class GameData : SaveData
{
    //public StageInfo[,] stageinfo = ;

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