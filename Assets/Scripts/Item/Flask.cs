using UnityEngine;

[CreateAssetMenu(menuName = "Items/Flask")]
public class Flask : Consumable
{
    [Header("Consumable Type")]
    public bool isHP;
    public bool isEXP;

    [Header("Amount")]
    public int amount;

    [Header("")]
    public GameObject fx;

    public override void AttemptToConsumableItem(AnimatorManager animatorManager, WeaponSlotManager weaponSlotManager, PlayerEffects playerEffects)
    {
        base.AttemptToConsumableItem(animatorManager, weaponSlotManager, playerEffects);
        GameObject flask = Instantiate(itemModel, weaponSlotManager.leftHand.position);
        // Thêm vào playereffects
        playerEffects.currentParticleFX = fx;
        // Tạo flask trên tay + animation
        playerEffects.amount = amount;
        playerEffects.instantiateFX = flask;
        weaponSlotManager.leftHand.UnloadWeapon();
        // 
    }
}
