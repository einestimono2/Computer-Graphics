using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffects : CharacterEffects
{
    PlayerStats playerStats;
    WeaponSlotManager weaponSlotManager;

    public GameObject currentParticleFX;
    public GameObject instantiateFX;

    public int amount;

    private void Awake() {
        playerStats = GetComponentInParent<PlayerStats>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    public void HealPlayerFromEffect(){
        playerStats.HealHP(amount);

        Destroy(instantiateFX.gameObject);

        weaponSlotManager.LoadAllWeapons();
    }
    
}
