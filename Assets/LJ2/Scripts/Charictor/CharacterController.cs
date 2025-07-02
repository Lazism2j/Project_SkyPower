using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LJ2
{
    public class CharactorController : MonoBehaviour
    {
        public CharacterData characterData;

        public SaveTester saveTester;

        public Parrying parrying;
        public Ultimate ultimate;

        public int id;
        public Grade grade;
        public string name;
        public Elemental elemental;

        public int level;
        public int step;
        public int exp;

        public int Hp;
        public int attackDamage;
        public float attackSpeed;
        public float moveSpeed;
        public int defense;

        public int ultLevel;
        public float ultDamage;
        public int ultCool;

        public GameObject bulletPrefab;
        public GameObject ultPrefab;

        public Parry parry;
        public int parryCool;
        public Sprite image;

        public int upgradeUnit;

        private void Start()
        {
            parrying = GetComponent<Parrying>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                id = characterData.id;
                SetParameter();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                LevelUp(5000);  // 5000�� ���÷�, ���� ���ӿ����� �÷��̾ ���� ���� ���� ���� �ٸ��� �����ؾ� ��
                SetParameter();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                StepUp();
                SetParameter();
            }
        }
        private void SetParameter()
        {
            // Data�� ���� �״�� ������
            // bulletPrefab = characterData.bulletPrefab;
            // ultPrefab = characterData.ultVisual;
            // image = charictorData.image;

            grade = characterData.grade;
            name = characterData.name;
            elemental = characterData.elemental;
            attackSpeed = characterData.attackSpeed;
            moveSpeed = characterData.moveSpeed;
            defense = characterData.defense;


            // Save�� ���� �״�� ������  

            CharacterSave characterSave = saveTester.gameData.characterInventory.characters.Find(c => c.id == id);

            if (characterSave.id == 0)
            {
                Debug.LogWarning($"id {id}�� �ش��ϴ� CharacterSave�� ã�� �� �����ϴ�.");
                return;
            }

            Debug.Log($"Character ID: {characterSave.id}, Step: {characterSave.step}, Level : {characterSave.level}");
            level = characterSave.level;
            step = characterSave.step;

            // Save�� ���� ���� Data�� ���� ����
            Hp = characterData.hp + (characterData.hpPlus * (level - 1));
            attackDamage = (int)(characterData.attackDamage + (characterData.damagePlus * (level - 1)));
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

                    int index = saveTester.gameData.characterInventory.characters.FindIndex(c => c.id == id);
                    CharacterSave characterSave = saveTester.gameData.characterInventory.characters[index];
                    characterSave.level = level;
                    saveTester.gameData.characterInventory.characters[index] = characterSave;
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

                int index = saveTester.gameData.characterInventory.characters.FindIndex(c => c.id == id);
                CharacterSave characterSave = saveTester.gameData.characterInventory.characters[index];
                characterSave.step = step;
                saveTester.gameData.characterInventory.characters[index] = characterSave;
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

        public void UseUlt(float ultDamage)
        {
            switch(id)
            {                
                case 10001:
                    ultimate.Laser();
                    break;
                case 10002:
                    // ����ź �̱���
                    break;
                case 10003:
                    // ź�� ���� ������ ����
                    break;
                case 10004:
                    // �ñر� ź�� 1ȸ - �ٴ���Ʈ
                    break;
                case 10005:
                    // �ñر� ź�� 1ȸ - �ٴ���Ʈ
                    break;
                case 10006:
                    defense += ultimate.Shield();
                    break;
                default:
                    ultimate.AllAttack();
                    break;
            }
        }
    }
}
