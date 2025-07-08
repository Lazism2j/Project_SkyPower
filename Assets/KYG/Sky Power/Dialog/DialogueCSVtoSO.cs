#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace KYG.SkyPower
{
    public class DialogCSVToSO
    {
        [MenuItem("Tools/Dialog/CSV to SO")]
        public static void Convert()
        {
            string path = "Assets/Dialog/dialog.csv";
            string[] lines = File.ReadAllLines(path);

            DialogDB db = ScriptableObject.CreateInstance<DialogDB>();
            db.lines = new List<DialogLine>();

            for (int i = 1; i < lines.Length; i++) // ù ���� ���
            {
                // 1. ���� Ȥ�� �ּ�(���� # ��) ����
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                string[] t = lines[i].Split(',');

                // 2. �ʵ� ���� ���� ���� (��: 6 �̸��̸� �ǳʶ�)
                if (t.Length < 4)
                {
                    Debug.LogWarning($"�� {i + 1}: �ʵ� �� ���� (�߰�: {t.Length}). ����: \"{lines[i]}\". �ǳʶݴϴ�.");
                    continue;
                }

                // 3. ���� �Ľ� �����ڵ�
                int idVal;
                if (!int.TryParse(t[0].Trim(), out idVal))
                {
                    Debug.LogWarning($"�� {i + 1}: id �Ľ� ����. ����: \"{lines[i]}\". �ǳʶݴϴ�.");
                    continue;
                }

                int nextVal = 0;
                string nextField = t[3].Trim();
                if (nextField != "END" && !int.TryParse(nextField, out nextVal))
                {
                    Debug.LogWarning($"�� {i + 1}: nextID �Ľ� ����. ����: \"{lines[i]}\". �ǳʶݴϴ�.");
                    continue;
                }

                var line = new DialogLine()
                {
                    id = idVal,
                    speaker = t[1].Trim(),
                    portrait = Resources.Load<Sprite>($"Portraits/{t[2].Trim()}"),
                    text = t[3].Trim(),
                    //sound = Resources.Load<AudioClip>($"Sounds/{t[4].Trim()}"),
                    //nextID = nextField == "END" ? 0 : nextVal
                };
                db.lines.Add(line);
            
        }
            AssetDatabase.CreateAsset(db, "Assets/Dialog/DialogDB.asset");
            AssetDatabase.SaveAssets();
            Debug.Log("DialogDB.asset ���� �Ϸ�");
        }
    }
}
#endif
