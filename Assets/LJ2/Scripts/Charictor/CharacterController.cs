using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KYG_skyPower;
using JYL;
using Unity.VisualScripting;

namespace LJ2
{
    [System.Serializable]
    public class CharactorController : MonoBehaviour
    {
        public CharacterData characterData;

        public Parrying parrying;
        public Ultimate ultimate;

        public int id => characterData.id;
        public Grade grade;
        public string charName;
        public Elemental elemental;

        public int level;
        public int step;
        public int exp;

        public int Hp;
        public int attackDamage;
        public float attackSpeed;
        public float moveSpeed;
        public int defense;
        public PartySet partySet;

        public int ultLevel;
        public float ultDamage;
        public int ultCool;

        public PooledObject bulletPrefab; // TODO : �������
        public PooledObject ultBulletPrefab;
        public GameObject ultPrefab; // ���ҽ�

        public Parry parry;
        public int parryCool;

        public Sprite icon;
        public Sprite image;

        public int upgradeUnit;

        private void Awake()
        {
            parrying = GetComponent<Parrying>();
            ultimate = GetComponent<Ultimate>();
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    id = characterData.id;
            //    SetParameter();
            //}

            //if (Input.GetKeyDown(KeyCode.L))
            //{
            //    LevelUp(5000);  // 5000�� ���÷�, ���� ���ӿ����� �÷��̾ ���� ���� ���� ���� �ٸ��� �����ؾ� ��
            //    SetParameter();
            //}

            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    StepUp();
            //    SetParameter();
            //}
        }

        public void ApplyEquipmentStat()
        {
            var equips = EquipmentInvenManager.Instance.GetEquippedItems(id);

            // �⺻�� ����
            attackDamage = (int)characterData.attackDamage;
            defense = characterData.defense;
            Hp = characterData.hp;

            // ����
            if (equips.weapon != null)
                attackDamage += equips.weapon.Base_Value;

            // ��
            if (equips.armor != null)
                defense += equips.armor.Base_Value;

            // �Ǽ�����
            if (equips.accessory != null)
            {
                // ����: ü�� ����
                Hp += equips.accessory.Base_Value;
                // ȿ�� Ÿ�Ժ��� �߰� ���� (Effect_Type ��)
            }
        }
        public void SetParameter()
        {
            // Data�� ���� �״�� ������
            // bulletPrefab = characterData.bulletPrefab;
            // ultPrefab = characterData.ultVisual;
            // image = charictorData.image;
            
            //EquipController weapon;
            //EquipController armor;
            //EquipController acce;
            //foreach(var item in equips)
            //{
            //    // 0�� = ����, 1���� ��, 2���� �Ǽ�
            //    switch(item.id)
            //    { 
            //    case Manager.Game.CurrentSave.equip[0].id:
            //        weapon = item;
            //        break;
            //    case Manager.Game.CurrentSave.equip[1].id:
            //        armor = item;
            //        break;
            //    case Manager.Game.CurrentSave.equip[2].id:
            //        acce = item;
            //        break;
            //    }
            //}

            grade = characterData.grade;
            charName = characterData.characterName;
            elemental = characterData.elemental;
            attackSpeed = characterData.attackSpeed;
            moveSpeed = characterData.moveSpeed;
            defense = characterData.defense;
            image = characterData.image;
            icon = characterData.icon;
            image = characterData.image;


            // Save�� ���� �״�� ������  

            CharacterSave characterSave = Manager.Game.saveFiles[Manager.Game.currentSaveIndex].characterInventory.characters.Find(c => c.id == id);

            if (characterSave.id == 0)
            {
                Debug.LogWarning($"id {id}�� �ش��ϴ� CharacterSave�� ã�� �� �����ϴ�.");
                return;
            }

            //Debug.Log($"Character ID: {characterSave.id}, Step: {characterSave.step}, Level : {characterSave.level}");
            level = characterSave.level;
            step = characterSave.step;
            bulletPrefab = Resources.Load<PooledObject>($"Prefabs/bullet/{id}_{step}");
            partySet = characterSave.partySet;

            // Save�� ���� ���� Data�� ���� ����
            if(partySet == PartySet.Main)
            {
                // TODO : ��� ���� �߰� ����
                //Hp = characterData.hp + (characterData.hpPlus * (level - 1)) + armor.hp;
                //attackDamage = (int)(characterData.attackDamage + (characterData.damagePlus * (level - 1)))+weapon.attackPower; 
            }
            else
            {
                Hp = characterData.hp + (characterData.hpPlus * (level - 1));
                attackDamage = (int)(characterData.attackDamage + (characterData.damagePlus * (level - 1)));
            }

            ultLevel = step + 1;
            ultCool = characterData.ultCoolDefault - (characterData.ultCoolReduce * step);
            upgradeUnit = characterData.upgradeUnitDefault + (characterData.upgradeUnitPlus * level);

            switch (id)
            {
                case 10001:
                    ultDamage = characterData.attackDamage * ((150 + 25 * Mathf.Pow(step, 2)) / 100);
                    break;
                case 10002:
                    ultDamage = characterData.attackDamage * ((120 + 20 * step) / 100);
                    break;
                case 10003:
                    ultDamage = characterData.attackDamage * ((150 + 50 * step) / 100);
                    break;
                case 10004:
                    ultDamage = characterData.attackDamage * ((130 + 30 * step) / 100);
                    break;
                case 10005:
                    ultDamage = characterData.attackDamage * ((150 + (12.5f * Mathf.Pow(step, 2)) + (37.5f * step)) / 100);
                    break;
                case 10006:
                    ultDamage = characterData.attackDamage * ((150 + (12.5f * Mathf.Pow(step, 2)) + (37.5f * step)) / 100);
                    break;
                default:
                    ultDamage = characterData.attackDamage * ((150 + 50 * step) / 100);
                    break;
            }
        }

        // ���׷��̵� ������ ���� ����
        public void LevelUp(int unit)
        {
            if (level < characterData.maxLevel)
            {
                
                if (unit > upgradeUnit)
                {
                    unit -= upgradeUnit;
                    level++;

                    int index = Manager.Game.CurrentSave.characterInventory.characters.FindIndex(c => c.id == id);
                    CharacterSave characterSave = Manager.Game.CurrentSave.characterInventory.characters[index];
                    characterSave.level = level;
                    Manager.Game.CurrentSave.characterInventory.characters[index] = characterSave;
                }
                else
                {
                    Debug.Log("���׷��̵� ������ �����մϴ�.");
                }

            }
            else
            {
                Debug.Log("�ִ� ������ �����߽��ϴ�.");
            }
        }

        public void GetUpgradeUnit(int unit)
        {
            exp += unit;
            if (exp > upgradeUnit)
            {
                Debug.Log("���׷��̵� �����մϴ�.");
            }
            else
            {
                Debug.Log("���׷��̵� ���� ������ �����մϴ�.");
            }
        }
        public void StepUp() 
        {
            if (step < 4)
            {
                step++;

                int index = Manager.Game.CurrentSave.characterInventory.characters.FindIndex(c => c.id == id);
                CharacterSave characterSave = Manager.Game.CurrentSave.characterInventory.characters[index];
                characterSave.step = step;
                Manager.Game.CurrentSave.characterInventory.characters[index] = characterSave;
            }
            else
            {
                Debug.Log("�ִ� �ܰ迡 �����߽��ϴ�.");
            }
        }

        public void UseParry()
        {
            // Parry ����� ����� ������ ��Ÿ���� üũ�ϰ� ����
            switch (parry)
            {
                case Parry.��:
                    parrying.Parry();
                    defense += parrying.Shield();
                    break;
                case Parry.�ݻ�B:
                    parrying.Parry();
                    // �ݻ� ��� �̱���
                    break;
                case Parry.����:
                    parrying.Parry();
                    parrying.Invicible();
                    break;
            }
        }

        public void UseUlt()
        {
            switch(id)
            {                
                case 10001:
                    ultimate.Laser(ultDamage);
                    break;
                case 10002:
                    // ����ź �̱���
                    break;
                case 10003:
                    // ź�� ���� ������ ����
                    // ultimate.BulletUpgrade();
                    break;
                case 10004:
                    // �ñر� ź�� 1ȸ - �ٴ���Ʈ
                    // ultimate.BigBullet(ultDamage);
                    break;
                case 10005:
                    // �ñر� ź�� 1ȸ - �ٴ���Ʈ
                    // ultimate.BigBullet(ultDamage);
                    break;
                case 10006:
                    defense += ultimate.Shield(ultDamage);
                    break;
                default:
                    ultimate.AllAttack(ultDamage);
                    break;
            }
        }
    }
}
