#if UNITY_EDITOR
using IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KYG_skyPower
{
    public class CsvToItemSO : MonoBehaviour
    {
        [Header("CSV ������ ���̺�")]
        public CsvTable table; // CSV ������ ���̺��� �����Ϳ��� �Ҵ�

        private const string assetPath = "Assets/KYG/Sky Power/Item/"; // ������ SO ���� ��ġ
        private const string prefabPath = "Assets/KYG/Sky Power/Prefabs/";

        [ContextMenu("CSV�κ��� ������ SO ����")]
        public void CreateItemsFromCSV()
        {
            if (table == null)
            {
                Debug.LogError("CSV ���̺��� �Ҵ���� �ʾҽ��ϴ�.");
                return;
            }

            CsvReader.Read(table); // (�ʿ��ϴٸ�) ���̺� ���ΰ�ħ

            for (int i = 1; i < table.Table.GetLength(0); i++) // 0���� ����̹Ƿ� 1���� ����
            {
                var item = ScriptableObject.CreateInstance<ItemData>();

                // ������ �Ľ� �� ���� ó��
                if (!int.TryParse(table.GetData(i, 0), out item.id))
                {
                    Debug.LogError($"ID �Ľ� ����: {table.GetData(i, 0)} (���� {i})");
                    continue;
                }
                item.itemName = table.GetData(i, 1);
                if (!int.TryParse(table.GetData(i, 2), out item.itemTime))
                    Debug.LogWarning($"itemTime �Ľ� ����: {table.GetData(i, 2)} (���� {i})");
                if (!int.TryParse(table.GetData(i, 3), out item.value))
                    Debug.LogWarning($"value �Ľ� ����: {table.GetData(i, 3)} (���� {i})");
                if (!int.TryParse(table.GetData(i, 4), out item.itemEffect))
                    Debug.LogWarning($"itemEffect �Ľ� ����: {table.GetData(i, 4)} (���� {i})");

                string prefabName = table.GetData(i, 5);
                if (!string.IsNullOrEmpty(prefabName))
                    item.itemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{prefabPath}{prefabName}.prefab");
                else
                    item.itemPrefab = null;

                item.description = table.GetData(i, 6);

                // ���ϸ� �� �ߺ� ���� ó��
                string fileName = $"{assetPath}{item.itemName}_{item.id}.asset";
                if (AssetDatabase.LoadAssetAtPath<ItemData>(fileName) != null)
                    AssetDatabase.DeleteAsset(fileName);

                AssetDatabase.CreateAsset(item, fileName);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("������ SO ���� �Ϸ�!");
        }
    }
}
#endif
