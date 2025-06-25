using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharictorController : MonoBehaviour
{
    public CharictorData charictorData;

    public CharictorSave charictorSave;

    public int level;
    public int Hp;
    public int exp;
    public int attackPower;
    public float attackSpeed;
    public float moveSpeed;
    public GameObject bulletPrefab;
    public GameObject model;
    public Sprite image;

    private void SetParameter()
    {
        // Data�� ���� �״�� ������
        bulletPrefab = charictorData.bulletPrefab;
        model = charictorData.model;
        image = charictorData.image;

        // Save�� ���� �״�� ������
        level = charictorSave.level;
        exp = charictorSave.exp;
    }

}
