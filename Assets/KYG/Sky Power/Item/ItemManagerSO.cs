using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    [CreateAssetMenu(menuName = "Manager/ItemManagerSO")]
    public class ItemManagerSO : ScriptableObject
    {
        [Header("������ ������ SO ����Ʈ (���� or �ڵ� �Ҵ�)")]
        public List<ItemData> allItems = new List<ItemData>();

        // ID�� �˻�
        public ItemData GetItemById(int id)
        {
            return allItems.Find(item => item != null && item.id == id);
        }

        // �̸����� �˻�
        public ItemData GetItemByName(string name)
        {
            return allItems.Find(item => item != null && item.itemName == name);
        }

#if UNITY_EDITOR
        // ������ �ڵ� ��� ��� (���� ���� ��� ItemData SO ���)
        [ContextMenu("�������� �ڵ����� ������ SO ��� ���")]
        public void AutoCollectAllItems()
        {
            allItems.Clear();
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:ItemData", new[] { "Assets/KYG/Sky Power/Item" });
            foreach (string guid in guids)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                ItemData item = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemData>(path);
                if (item != null && !allItems.Contains(item))
                    allItems.Add(item);
            }
            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log($"[ItemManagerSO] {allItems.Count}�� SO �ڵ� ��� �Ϸ�!");
        }
#endif
    }
}
