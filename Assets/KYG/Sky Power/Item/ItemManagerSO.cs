using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    [CreateAssetMenu(menuName = "Manager/ItemManagerSO")]
    public class ItemManagerSO : ScriptableObject // ������ ���� �Ŵ��� SO (������ ������ SO�� �����ϴ� ��ũ��Ʈ ������Ʈ) 
    {
        [Header("������ ������ SO ����Ʈ (���� or �ڵ� �Ҵ�)")]
        public List<ItemData> allItems = new List<ItemData>(); // ������ ������ SO�� �����ϴ� ����Ʈ (���� �Ǵ� �ڵ����� �Ҵ�)

        // ID�� �˻�
        public ItemData GetItemById(int id) // ������ ID�� �˻��ϴ� �޼���
        {
            return allItems.Find(item => item != null && item.id == id); // ������ ID�� �˻��ϴ� �޼���
        }

        // �̸����� �˻�
        public ItemData GetItemByName(string name) // ������ �̸����� �˻��ϴ� �޼���
        {
            return allItems.Find(item => item != null && item.itemName == name); // ������ �̸����� �˻��ϴ� �޼���
        }

#if UNITY_EDITOR
        // ������ �ڵ� ��� ��� (���� ���� ��� ItemData SO ���)
        [ContextMenu("�������� �ڵ����� ������ SO ��� ���")] 
        public void AutoCollectAllItems() // �����Ϳ����� �����ϴ� �ڵ� ��� �޼���
        {
            allItems.Clear(); // ���� ������ ����Ʈ �ʱ�ȭ
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:ItemData", new[] { "Assets/KYG/Sky Power/Item" }); // "Assets/KYG/Sky Power/Item" ���� ���� ��� ItemData SO�� �˻�
            foreach (string guid in guids) // �˻��� GUID�� ��ȸ�ϸ�
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid); // GUID�� ��η� ��ȯ
                ItemData item = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemData>(path); // ��ο��� ItemData SO�� �ε�
                if (item != null && !allItems.Contains(item)) // �������� null�� �ƴϰ� ����Ʈ�� ���ԵǾ� ���� ������
                    allItems.Add(item); // �������� ����Ʈ�� �߰�
            }
            UnityEditor.EditorUtility.SetDirty(this); // ���� ���� ����
            Debug.Log($"[ItemManagerSO] {allItems.Count}�� SO �ڵ� ��� �Ϸ�!"); // ��� �Ϸ� �޽��� ���
        }
#endif
    }
}
