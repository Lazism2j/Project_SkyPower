using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace KYG.SkyPower.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public DialogDB dialogDB;                         // SO�� ���
        public UnityEvent<DialogLine> OnDialogLine;       // UI/����/�ִϿ��� ����

        private Dictionary<int, DialogLine> lineDict;     // ID�� ������ �˻�
        private int currentID;                            // ���� ��� ID
        public bool IsActive { get; private set; }        // ��ȭ ������?

        void Awake()
        {
            lineDict = new Dictionary<int, DialogLine>();
            foreach (var line in dialogDB.lines)
            {
                if (line != null)
                    lineDict[line.id] = line;
            }
        }

        public void StartDialog(int startID)
        {
            currentID = startID;
            IsActive = true;
            ShowLine();
        }

        public void Next()
        {
            if (!IsActive) return;
            if (!lineDict.ContainsKey(currentID)) { EndDialog(); return; }

            var line = lineDict[currentID];
            OnDialogLine?.Invoke(line);

            if (line.nextID == 0 || !lineDict.ContainsKey(line.nextID))
                EndDialog();
            else
                currentID = line.nextID;
        }

        void ShowLine()
        {
            if (!lineDict.ContainsKey(currentID)) { EndDialog(); return; }
            OnDialogLine?.Invoke(lineDict[currentID]);
        }

        void EndDialog()
        {
            IsActive = false;
            OnDialogLine?.Invoke(null);
        }
    }
}
