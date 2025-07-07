using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KYG.SkyPower.Dialogue
{
    // ��ȭ ������ ��ũ���ͺ� ������Ʈ
    // ��ȭ ����, ȭ��, �ʻ�ȭ �� �ʿ��� ������ ��´�.
    // �����Ϳ��� ���� ���� �����ϵ��� �Ѵ�.

[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/DialogueData")]
public class DialogueDataSO : ScriptableObject
{
    [System.Serializable]
    public struct Line
    {
        public string speaker;
        [TextArea] public string content;
        // �ʿ�� �ʻ�ȭ, ����Ʈ �� �߰�
    }
        public List<Line> lines = new List<Line>();
    }
}