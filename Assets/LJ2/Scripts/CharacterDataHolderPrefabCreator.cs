using UnityEngine;
using UnityEditor;
using System.Linq;
using LJ2;

public class CharacterDataHolderPrefabCreator
{
    [MenuItem("Tools/CharacterDataHolder ������ �ڵ�����")]
    public static void CreatePrefabsForAllCharacterData()
    {
        // ��� CharacterData ScriptableObject ���� ��� ã��  
        string[] guids = AssetDatabase.FindAssets("t:CharacterData", new[] { "Assets/LJ2/Scripts/Charictor" });
        var saveDir = "Assets/LJ2/Prefabs/CharacterDataHolders";
        if (!AssetDatabase.IsValidFolder(saveDir))
            AssetDatabase.CreateFolder("Assets/LJ2/Prefabs", "CharacterDataHolders");

        foreach (var guid in guids)
        {
            var data = AssetDatabase.LoadAssetAtPath<CharacterData>(AssetDatabase.GUIDToAssetPath(guid));
            if (data == null) continue;

            GameObject go = new GameObject($"{data.name}");
            var holder = go.AddComponent<LJ2.CharactorController>(); // Fix: Changed from CharacterController to CharacterDataHolder  
            holder.characterData = data;

            string prefabPath = $"{saveDir}/{data.name}.prefab";
            PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
            Object.DestroyImmediate(go);
        }

        Debug.Log("�� CharacterData���� CharacterDataHolder ������ �ڵ����� �Ϸ�");
    }
}
