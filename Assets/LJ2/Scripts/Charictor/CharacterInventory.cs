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
        this.level = 1; // Default level
        this.step = 0; // Default step
        equipId = new int[3];
        equipId[0] = -1;
        equipId[1] = -1;
        equipId[2] = -1;
        partySet = PartySet.None;
    }
}
public enum PartySet { None, Main, Sub1, Sub2 }