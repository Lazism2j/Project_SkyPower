using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IO;

public class CsvCharictorController : MonoBehaviour
{
    public CharictorHasCsv charictorHasCsv;

    // ���̺� ��� �ʿ��� ����
    public int level = 1;
    public int exp;

    // �ΰ��� �ʿ� ����
    public int Hp;
    public int attackPower;
    public int attackSpeed;
    public int moveSpeed;

    private void Start()
    {
        // ToDo ���� ��Ȳ �ε��Ͽ� level, exp��������
        GetParmeter(level);
    }
    private void GetParmeter(int level)
    {
        CsvReader.Read(charictorHasCsv.dataTable);

        Hp = int.Parse(charictorHasCsv.dataTable.GetData(level, 1));
        attackPower = int.Parse(charictorHasCsv.dataTable.GetData(level, 2));
        attackSpeed = int.Parse(charictorHasCsv.dataTable.GetData(level, 3));
        moveSpeed = int.Parse(charictorHasCsv.dataTable.GetData(level, 4));
    }
}
