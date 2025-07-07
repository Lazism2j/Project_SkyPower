using KYG.SkyPower.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;


namespace KYG.SkyPower.Dialogue
{
    // CSV ������ �о� ScriptableObject�� ��ȯ�ϴ� ������ ��ũ��Ʈ
    // CSV ������ "speaker, content" �������� �Ǿ� �־�� �մϴ�.
    // ����: "Player, Hello World!"

public class DialogueCSVtoSO
{
    [MenuItem("Tools/CSV/Convert Dialogue CSV to SO")]
    public static void Convert()
    {
        // 1. ��� ���� (�ʿ�� ��� ����)
        string csvPath = "Assets/Dialogues/Dialogue_Stage1.csv";
        string soPath = "Assets/Dialogues/Dialogue_Stage1_SO.asset";

        if (!File.Exists(csvPath))
        {
            Debug.LogError("CSV ������ ã�� �� �����ϴ�: " + csvPath);
            return;
        }

        // 2. CSV �б�
        var lines = File.ReadAllLines(csvPath);

        // 3. DialogueDataSO ����
        var dialogueSO = ScriptableObject.CreateInstance<DialogueDataSO>();
        dialogueSO.lines = new List<DialogueDataSO.Line>();

        // 4. CSV �Ľ� (1�پ�)
        for (int i = 1; i < lines.Length; i++) // 0��°�� ���
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
            var tokens = lines[i].Split(',');

            if (tokens.Length < 2) continue; // speaker, content
            DialogueDataSO.Line line = new DialogueDataSO.Line
            {
                speaker = tokens[0].Trim(),
                content = tokens[1].Trim().Replace("\\n", "\n")
            };
            dialogueSO.lines.Add(line);
        }

        // 5. SO ���� ����/����
        AssetDatabase.CreateAsset(dialogueSO, soPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Dialogue SO ���� �Ϸ�: {soPath}");
    }
}
}