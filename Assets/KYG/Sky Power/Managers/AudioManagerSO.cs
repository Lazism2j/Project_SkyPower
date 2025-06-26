using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AudioManagerSO", menuName = "Manager/AudioManager")]
public class AudioManagerSO : ScriptableObject
{
    private static AudioManagerSO instance;

    public static AudioManagerSO Instance // �̱��� ����
    {
        get
        {
            if (!instance)
                instance = Resources.Load<AudioManagerSO>("AudioManagerSO")
                    return instance;
        }
    }

    [Header("���� �����ͺ��̽�")]
    public AudioDataBase audioDB;

    [Header("����Ʈ BGM / SFX")] // ���� ���۽� �⺻ ����
    public AudioData defaultBGM;
    public AudioData defaultSFX;

    private Dictionary<string, AudioData> audioDict; // �̸����� ������ ã�� ��ųʸ�

    public void Init() // ����� DB ��ŷ����� �ʱ�ȭ
    {
        audioDict = new Dictionary<string, AudioData>();
        foreach (var data in audioDB.audioList)
        {
            if (!audioDict.ContainsKey(data.clipName))
                audioDict.Add(data.clipName, data);
        }
    }

    public AudioData GetAudioData(string name) // �̸����� �����DB ��ȯ 
    {
        if (audioDict == null) Init();
        audioDict.TryGetValue(name, out AudioData data);
        return data;
    }
}
