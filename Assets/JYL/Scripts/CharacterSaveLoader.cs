using LJ2;
using System;
using UnityEngine;

namespace JYL
{
    public class CharacterSaveLoader : MonoBehaviour
    {
        public CharactorController[] charactorController;
        public CharactorController mainController;
        public CharactorController sub1Controller;
        public CharactorController sub2Controller;
        // public EquipSaveLoader equipLoader;
        private string charPrefabPath = "CharacterPrefabs";

        // TODO : ��� �Ǵ��� �׽�Ʈ
        //void OnEnable()
        //{
        //    GetCharPrefab();
        //}

        void Update() { }
        public void GetCharPrefab()
        {
            //ĳ���� ������ ���� ��������
            charactorController = Resources.LoadAll<CharactorController>(charPrefabPath);
            foreach (CharactorController cont in charactorController)
            {
                cont.SetParameter(); // TODO : ��Ţ�δ� �ϼ��Ǹ� ���⿡ ���� equipLoader
                switch(cont.partySet)
                {
                    case PartySet.Main:
                        mainController = cont;
                        break;
                    case PartySet.Sub1:
                        sub1Controller = cont;
                        break;
                    case PartySet.Sub2:
                        sub2Controller = cont;
                        break;
                }
            }
            // ���� ���Ķ���� ��.
            Array.Sort(charactorController, (a, b) => a.partySet.CompareTo(b.partySet)); // �߰����� ���ĵ� ������.

        }
    }
}
