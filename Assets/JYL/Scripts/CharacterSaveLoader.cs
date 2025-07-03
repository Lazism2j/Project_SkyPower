using LJ2;
using System;
using UnityEngine;

namespace JYL
{
    public class CharacterSaveLoader : MonoBehaviour
    {
        public CharactorController[] charactorController;

        private string charPrefabPath = "CharacterPrefabs";

        void OnEnable()
        {
            GetCharPrefab();
        }

        void Update() { }
        public void GetCharPrefab()
        {
            //ĳ���� ������ ���� ��������
            charactorController = Resources.LoadAll<CharactorController>(charPrefabPath);
            foreach (var cont in charactorController)
            {
                cont.SetParameter();
            }
            // ���� ���Ķ���� ��.
            Array.Sort(charactorController, (a, b) => a.partySet.CompareTo(b.partySet)); // �߰����� ���ĵ� ������.
        }
    }
}
