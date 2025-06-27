using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    [CreateAssetMenu(menuName = "Game/DialogDataSO")] // DialogData ��ü �����ϴ� SO
    public class DialogDataSO : ScriptableObject
    {
        public List<DialogLine> lines = new List<DialogLine>();

        private Dictionary<int, DialogLine> dict; // ID�� ��ȸ
        public void Init() // lines -> dict ��ȯ
        {
            dict = new Dictionary<int, DialogLine>();
            foreach (var l in lines) dict[l.id] = l;
        }
        public DialogLine GetLine(int id) // ID�� ��� ���� ��ȸ
        {
            if (dict == null || dict.Count != lines.Count) Init();
            return dict.TryGetValue(id, out var line) ? line : null;
        }
    }
}