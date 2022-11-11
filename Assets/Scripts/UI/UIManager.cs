using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerInventory playerInventory;

    [Header("UI")]
    public GameObject inventoryUI;
    public GameObject hudUI;

    [Header("Inventory Slot")]
    public Transform inventorySlotParent;
    public GameObject inventorySlotPrefab;
    InventorySlot[] inventorySlots;

    [Header("Equipment Slot")]
    public Transform equipmentSlotParent;
    InventorySlot[] equipmentSlots;

    void Start(){
        inventorySlots = inventorySlotParent.GetComponentsInChildren<InventorySlot>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<InventorySlot>();
    }

    public void LoadSlots(){
        // Weapons In Inventory
        for(int i = 0; i < inventorySlots.Length; i++){
            if(i < playerInventory.weaponsInventory.Count){
                if(inventorySlots.Length < playerInventory.weaponsInventory.Count){
                    Instantiate(inventorySlotPrefab, inventorySlotParent);
                    inventorySlots = inventorySlotParent.GetComponentsInChildren<InventorySlot>();
                }
                inventorySlots[i].AddItem(playerInventory.weaponsInventory[i]);
            }else{
                inventorySlots[i].RemoveItem();
            }
        }

        // Equipment (Right - Left - )
        if(playerInventory.rightWeapon != null) equipmentSlots[0].AddItem(playerInventory.rightWeapon);
        if(playerInventory.leftWeapon != null) equipmentSlots[1].AddItem(playerInventory.leftWeapon);
    }
    
    public void OpenInventory(){
        inventoryUI.SetActive(true);
        hudUI.SetActive(false);
    }

    public void CloseInventory(){
        inventoryUI.SetActive(false);
        hudUI.SetActive(true);
    }

}
