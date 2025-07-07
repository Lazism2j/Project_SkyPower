using System.Collections.Generic;
using UnityEngine;

namespace KYG.SkyPower.Dialogue
{
    [System.Serializable]
    public class DialogLine
    {
        public int id;                      // ��� ������ȣ
        public string speaker;              // ȭ�ڸ�
        public Sprite portrait;             // �ʻ�ȭ
        [TextArea] public string text;      // ��� �ؽ�Ʈ
        public AudioClip sound;             // ����(����)
        public int nextID;                  // ���� ��� id (0 �Ǵ� -1�� ����)
    }

    [CreateAssetMenu(fileName = "DialogDB", menuName = "Dialog/DialogDB")]
    public class DialogDB : ScriptableObject
    {
        public List<DialogLine> lines;
    }
}
