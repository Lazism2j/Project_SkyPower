using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        public DialogDataSO dialogData; // �����Ϳ��� �Ҵ� or Resources.Load

        DialogLine currentLine;
        int currentId = 0; // ��ȭ ���� id(0�̳� ���ϴ� ��)

        public override void Init()
        {
            if (dialogData != null)
                dialogData.Init();
        }

        public void StartDialogue(int startId = 0)
        {
            currentId = startId;
            ShowLine(currentId);
        }

        void ShowLine(int id)
        {
            currentLine = dialogData.GetLine(id);
            if (currentLine == null)
            {
                Debug.Log("��ȭ ����!");
                // UI ����, �̺�Ʈ �ݹ� ��
                return;
            }

            // 1. �ؽ�Ʈ, ȭ��, �ʻ�ȭ �� UI�� ǥ��
            Debug.Log($"{currentLine.speaker}: {currentLine.text}");

            // 2. Portrait: UIManager ��� ���� (����)
            // var portrait = Resources.Load<Sprite>(currentLine.portraitKey);
            // portraitImage.sprite = portrait;

            // 3. ���̽�: AudioManager ��� ����
            // AudioManager.Instance.PlayVoice(currentLine.voiceKey);

            // 4. �б� ������(������ ��ư ǥ��)
            if (currentLine.isBranch)
            {
                ShowBranchOptions(currentLine.nextIds);
            }
            else
            {
                // "����" ��ư or �Է� ���
                // UI���� NextLine() ȣ��
            }
        }

        public void NextLine()
        {
            if (currentLine == null) return;
            if (currentLine.nextIds.Count > 0)
                ShowLine(currentLine.nextIds[0]);
            else
                Debug.Log("��ȭ ����");
        }

        void ShowBranchOptions(List<int> nextIds)
        {
            for (int i = 0; i < nextIds.Count; i++)
            {
                var nextLine = dialogData.GetLine(nextIds[i]);
                Debug.Log($"������ {i + 1}: {nextLine.text}");
                // ����: ��ư UI ����/�Ҵ� �� OnClick�� SelectBranch(i) ����
            }
        }

        public void SelectBranch(int branchIndex)
        {
            if (currentLine.isBranch && branchIndex < currentLine.nextIds.Count)
                ShowLine(currentLine.nextIds[branchIndex]);
        }
    }
}
