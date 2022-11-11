using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    WeaponSlotManager weaponSlotManager;

    public Consumable currentConsumable;

    public WeaponData rightWeapon;
    public WeaponData leftWeapon;

    public WeaponData unarmedWeapon;

    public List<WeaponData> weaponsInventory;

    // public WeaponData[] 

    void Awake(){
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    // Start is called before the first frame update
    void Start(){
        weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
