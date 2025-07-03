using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IO;

namespace KYG_skyPower
{
    public class SelfInventoryUITest : MonoBehaviour 
    {
        public InventoryManagerSO inventoryManagerSO; // Reference to the InventoryManagerSO
        public EquipmentTableSO equipmentTableSO; // Reference to the EquipmentTableSO
        public CharacterController testCharacter; // Reference to the CharacterController for testing
        public GameObject slotPrefab;
        public Transform weaponPanel, armorPanel, accessoryPanel; 

        // Added a new property to resolve the 'saveData' issue
        public SaveData saveData;

        void Start() { RefreshAll(); }

        void RefreshAll()
        {
            RefreshPanel("weapon", weaponPanel, 0);
            RefreshPanel("armor", armorPanel, 1);
            RefreshPanel("accessory", accessoryPanel, 2);
        }

        void RefreshPanel(string type, Transform panel, int slotIndex)
        {
            foreach (Transform child in panel) Destroy(child.gameObject);

            var list = inventoryManagerSO.inventory
                .Select(x => x.itemData)
                .OfType<EquipmentData>()
                .Where(x => x.Equip_Type == type)
                .ToList();

            foreach (var equip in list)
            {
                var go = Instantiate(slotPrefab, panel);
                var slotUI = go.GetComponent<InventorySlotUI>();
                bool isEquipped = saveData.equipId[slotIndex] == equip.Equip_Id; // Updated to use 'saveData'
                slotUI.Set(equip, isEquipped, (data) =>
                {
                    saveData.equipId[slotIndex] = (int)data.Equip_Id; // Updated to use 'saveData'
                    RefreshAll();
                });
            }
        }
    }

    // Example SaveData class definition (if not already defined elsewhere)
    public class SaveData
    {
        public int[] equipId = new int[3]; // Assuming 3 slots for weapon, armor, and accessory
    }
}