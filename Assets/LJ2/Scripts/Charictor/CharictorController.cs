using LJ2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharictorController : MonoBehaviour
{
    public CharictorDataTest charictorData;

    public CharictorSave charictorSave = new();

    public int level;
    public int Hp;
    public int exp;
    public int attackPower;
    public float attackSpeed;
    public float moveSpeed;
    public GameObject bulletPrefab;
    public GameObject model;
    public Sprite image;

    private void Start()
    {
        // ������ġ�� ���� index ��ȭ ���� �ʿ�
        SaveManager.Instance.PlayerLoad(charictorSave, 0);
    }
    private void SetParameter()
    {
        // Data�� ���� �״�� ������
        // bulletPrefab = charictorData.bulletPrefab;
        // model = charictorData.model;
        // image = charictorData.image;

        // Save�� ���� �״�� ������
        level = charictorSave.level;
        exp = charictorSave.exp;
    }

}
