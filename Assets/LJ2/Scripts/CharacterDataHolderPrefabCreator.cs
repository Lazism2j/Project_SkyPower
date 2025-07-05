using JYL;
using LJ2;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class CharacterDataHolderPrefabCreator
{
#if UNITY_EDITOR


    [MenuItem("Tools/CharacterDataHolder ������ �ڵ�����")]
    public static void CreatePrefabsForAllCharacterData()
    {
        // ��� CharacterData ScriptableObject ���� ��� ã��  
        string[] guids = AssetDatabase.FindAssets("t:CharacterData", new[] { "Assets/LJ2/Scripts/Charictor" });
        var saveDir = "Assets/Resources/CharacterPrefabs";
        if (!AssetDatabase.IsValidFolder(saveDir))
            AssetDatabase.CreateFolder("Assets/LJ2/Prefabs", "CharacterDataHolders");

        var erasePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/LJ2/Prefabs/Erase.Prefab");
        var laserPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/LJ2/Prefabs/Laser.Prefab");
        var shieldPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/LJ2/Prefabs/Shield.Prefab");
        var firePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/LJ2/Prefabs/Fire.Prefab");

        foreach (var guid in guids)
        {
            var data = AssetDatabase.LoadAssetAtPath<CharacterData>(AssetDatabase.GUIDToAssetPath(guid));
            if (data == null) continue;

            // �� GameObject ���� �� CharactorController ������Ʈ �߰�
            GameObject go = new GameObject($"{data.characterName}");
            var holder = go.AddComponent<LJ2.CharactorController>();  
            holder.characterData = data;

            // �ʿ��� ��� �߰� ������Ʈ ����
            var parry = go.AddComponent<Parrying>();  // Parrying ������Ʈ �߰�
            holder.parrying = parry;  // CharactorController�� Parrying ������Ʈ ����
            var ultimate = go.AddComponent<Ultimate>();  // Ultimate ������Ʈ �߰�
            holder.ultimate = ultimate;  // CharactorController�� Ultimate ������Ʈ ����
            holder.bulletPrefab = data.bulletPrefab.GetComponent<BulletPrefabController>(); // Bullet ������ ����

            ultimate.ultAll = erasePrefab; // Erase ������ ����
            ultimate.ultLaser = laserPrefab; // Laser ������ ����
            ultimate.ultFire = firePrefab; // Fire ������ ����
            ultimate.shield = shieldPrefab; // Shield ������ ����

            var characterModel = (GameObject)PrefabUtility.InstantiatePrefab(data.characterModel);
            var eraseObject = (GameObject)PrefabUtility.InstantiatePrefab(erasePrefab);
            var laserObject = (GameObject)PrefabUtility.InstantiatePrefab(laserPrefab);
            var shieldObject = (GameObject)PrefabUtility.InstantiatePrefab(shieldPrefab);
            var fireObject = (GameObject)PrefabUtility.InstantiatePrefab(firePrefab);

            characterModel.transform.SetParent(go.transform); // CharacterModel �������� ������ GameObject�� �ڽ����� ����
            eraseObject.transform.SetParent(go.transform); // Erase �������� ������ GameObject�� �ڽ����� ����
            laserObject.transform.SetParent(go.transform); // Laser �������� ������ GameObject�� �ڽ����� ����
            shieldObject.transform.SetParent(go.transform); // Shield �������� ������ GameObject�� �ڽ����� ����
            fireObject.transform.SetParent(go.transform); // Fire �������� ������ GameObject�� �ڽ����� ����

            string prefabPath = $"{saveDir}/{data.characterName}.prefab";
            PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
            Object.DestroyImmediate(go);
        }

        Debug.Log("�� CharacterData���� CharacterDataHolder ������ �ڵ����� �Ϸ�");
    }
    #endif


}
