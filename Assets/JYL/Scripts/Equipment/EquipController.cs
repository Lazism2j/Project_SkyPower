using KYG_skyPower;
using System.Collections.Generic;
using UnityEngine;

// ���â, �ΰ��� �������� ������ ����ϴ� ��ũ��Ʈ
public class EquipController : MonoBehaviour
{
    private EquipmentTableSO equipTable;
    public EquipInfo[] equipData;
    private string tableSOPath = "Inventory/EquipmentTableSO";
    private void Awake()
    {
        Init();
    }
    void Start()
    {

    }

    void Update()
    {

    }
    private void Init()
    {
        equipTable = Resources.Load<EquipmentTableSO>(tableSOPath);
        equipData = new EquipInfo[equipTable.equipmentList.Count];
        CreateEquipInfo();
    }
    public void CreateEquipInfo() // �������� ������ ��� �����͹迭�� ������. SO�� �ʱⵥ���͸� ������
    {
        int index = 0;
        foreach (EquipmentDataSO equip in equipTable.equipmentList)
        {
            EquipInfo equipInfo = new EquipInfo();
            equipInfo.id = equip.id;
            equipInfo.level = equip.level;
            equipInfo.name = equip.name;
            equipInfo.type = equip.type;
            equipInfo.grade = equip.grade;
            equipInfo.setType = equip.setType;
            equipInfo.icon = equip.icon;
            equipInfo.maxLevel = equip.maxLevel;
            equipInfo.originGold = equip.upgradeGold;
            equipInfo.upgradeGold = equip.upgradeGold;
            equipInfo.upgradeGoldPlus = equip.upgradeGoldPlus;
            equipInfo.equipValue = equip.equipValue;
            equipInfo.originValue = equip.equipValue;
            equipInfo.equipValuePlus = equip.equipValuePlus;
            equipInfo.Effect_Desc = equip.Effect_Desc;
            
            equipData[index] = equipInfo;
            
            index++;
        }
    }
    public void SaveFileInit() //���̺����� ���� ���� �� �۾� SaveCreatePanel���� ����.
    {
        int index = 0;

        Manager.Game.CurrentSave.equipInfo = new EquipSave[equipData.Length];
        foreach (EquipInfo data in equipData)
        {
            EquipSave tmp = new();
            tmp.id = data.id;
            tmp.level = -1;

            Manager.Game.CurrentSave.equipInfo[index] = tmp;

            index++;
        }
    }
    public void UpdateEquipInfo() // ��� ���� ������͸� �ֽ�ȭ. ���̺� �ҷ��ö� ���.
    {
        for (int i = 0; i < equipData.Length; i++)
        {
            if (equipData[i].id == Manager.Game.CurrentSave.equipInfo[i].id)
            {
                EquipInfo tmpInfo = equipData[i];
                EquipSave tmpSave = Manager.Game.CurrentSave.equipInfo[i];
                tmpSave.level = equipData[i].level;
                tmpInfo.upgradeGold = equipData[i].level * equipData[i].upgradeGoldPlus;
                tmpInfo.equipValue = equipData[i].originValue + (equipData[i].level - 1) * equipData[i].equipValuePlus;
                equipData[i] = tmpInfo;
                Manager.Game.CurrentSave.equipInfo[i] = tmpSave;
            }
            else
            {
                Debug.LogError($"�迭���� ��߳�: {equipData[i].id}, {Manager.Game.CurrentSave.equipInfo}");
            }
        }
    }
    public void UpdateEquipInfo(int id, bool Upgrade = false) // ��� ���׷��̵� ����
    {
        for (int i = 0; i < equipData.Length; i++)
        {
            if (equipData[i].id == id)
            {
                EquipSave tmpSave = Manager.Game.CurrentSave.equipInfo[i];
                EquipInfo tmpInfo = equipData[i];
                if (Upgrade) tmpInfo.level++; // maxLevel�� ������, UI�󿡼� ��� ���ƾ� ��;
                tmpSave.level = tmpInfo.level;
                tmpInfo.upgradeGold = equipData[i].level * equipData[i].upgradeGoldPlus;
                tmpInfo.equipValue = equipData[i].originValue + (equipData[i].level - 1) * equipData[i].equipValuePlus;
                Manager.Game.CurrentSave.equipInfo[i] = tmpSave;
                equipData[i] = tmpInfo;
                return;
            }
        }
    }
    public void AddEquipment(int id)
    {
        for (int i = 0; i < equipData.Length; i++)
        {
            if (equipData[i].id == id)
            {
                if (equipData[i].level > 0)
                {
                    Manager.Game.CurrentSave.gold += 10;
                }

                else
                {
                    EquipInfo tmpInfo = equipData[i];
                    tmpInfo.level = 1;
                    EquipSave tmp = Manager.Game.CurrentSave.equipInfo[i];
                    tmp.level = 1;
                    Manager.Game.CurrentSave.equipInfo[i] = tmp;
                    equipData[i] = tmpInfo;
                }

                return;
            }
        }
    }
    public List<EquipInfo> GetEquipListByType(EquipType type)
    {
        List<EquipInfo> result = new List<EquipInfo>();
        foreach (EquipInfo info in equipData)
        {
            if (info.type == type)
            {
                EquipInfo temp = info;
                result.Add(temp);
            }
        }
        return result;
    }
}
public struct EquipInfo
{
    public int index;
    public int id;
    public string name;
    public int level;
    public EquipType type;
    public EquipGrade grade;
    public SetType setType;
    public Sprite icon;
    public int maxLevel;
    public int originGold;
    public int upgradeGold;
    public int upgradeGoldPlus;
    public int originValue;
    public int equipValue;
    public int equipValuePlus;
    public string Effect_Desc;
}