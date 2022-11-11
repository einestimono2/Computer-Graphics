using UnityEngine;

// fileName: Tên tệp mặc định khi tạo mới loại này.
// menuName: Tên hiển thị cho loại này được hiển thị trong Assets/Create menu
[CreateAssetMenu(fileName = "WeaponData", menuName = "Items/Weapon")]
public class WeaponData : Item
{
    public GameObject weaponPrefab;
    public bool isUnarmed;

    [Header("Weapon Type")]
    public WeaponType weaponType;

    [Header("Damage")]
    public int baseDamage = 25;

    [Header("Absorption")]
    public float damageAbsorption = 0;

    [Header("Animator")]
    public AnimatorOverrideController weaponController;
}
