using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public bool isDead;

    public int level = 1;
    
    [Header("HP")]
    public int healthByLevel = 10;
    public float maxHealth = 100;
    public float currentHealth = 100;

    [Header("EXP")]
    public int expByLevel = 10;
    public int levelEXP = 100;
    public int currentEXP = 0;

    [Header("Currency")]
    public int coin = 0;
}
