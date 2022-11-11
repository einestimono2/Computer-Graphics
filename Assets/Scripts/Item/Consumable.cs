using UnityEngine;

public class Consumable : Item
{
    [Header("Item Quantity")]
    public int maxAmount;
    public int currentAmount;

    [Header("Model")]
    public GameObject itemModel;

    [Header("Animation")]
    public string animation;
    public bool isInteracting;

    public virtual void AttemptToConsumableItem(AnimatorManager animatorManager, WeaponSlotManager weaponSlotManager, PlayerEffects playerEffects){
        if(currentAmount > 0){
            animatorManager.PlayAnimation(animation, isInteracting);
        }else{
            animatorManager.PlayAnimation("Shrug", true);
        }
    }

}
