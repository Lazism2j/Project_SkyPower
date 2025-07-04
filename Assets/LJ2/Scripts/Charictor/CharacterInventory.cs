using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterInventory
{
    [SerializeField] public List<CharacterSave> characters;

    public CharacterInventory()
    {
        // Initialize the character list
        characters = new List<CharacterSave>();
    }
    public void AddCharacter(int id)
    {
        // Check if the character already exists in the inventory
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].id == id)
            {
                if (characters[i].step < 4)
                {
                    var temp = characters[i];
                    temp.step++;
                    characters[i] = temp; // Update the character in the list
                    return; // Exit if character already exists
                }
                else
                {
                    // ToDo : ��ȭ�� ��ȯ 
                    return; // Exit if character has reached maximum fragle level
                }
            }

        }

        characters.Add(new CharacterSave(id));
    }

}
[System.Serializable]
public struct CharacterSave
{
    [SerializeField] public int id;
    [SerializeField] public int level;

    [SerializeField] public int step;
    [SerializeField] public PartySet partySet;
    [SerializeField] public int[] equipId;
    public CharacterSave(int id)
    {
        this.id = id;
        switch(id)
        {
            case 10009:
                partySet = PartySet.Main;
                level = 1;
                break;
            case 10015:
                partySet = PartySet.Sub1;
                level = 1;
                break;
            case 10027:
                partySet = PartySet.Sub2;
                level = 1;
                break;
            default:
                level = -1;// Default level // ���� ��, 1������ ����
                partySet = PartySet.None;
                break;
        }
        this.step = 0; // Default step
        equipId = new int[3];
        equipId[0] = -1;
        equipId[1] = -1;
        equipId[2] = -1;
    }
}
public enum PartySet { Main, Sub1, Sub2, None }