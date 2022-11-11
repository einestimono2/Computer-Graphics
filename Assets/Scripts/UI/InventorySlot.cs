using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    WeaponData item;

    public void AddItem(WeaponData newItem){
        item = newItem;
        icon.sprite = newItem.itemIcon;
        icon.enabled = true;
        gameObject.SetActive(true);
    }

    public void RemoveItem(){
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        gameObject.SetActive(false);
    }
}
