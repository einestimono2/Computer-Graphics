using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : CharacterStats
{
    [Header("UI")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public Slider expSlider;

    public TextMeshProUGUI coinText;

    AnimatorHandler animatorHandler;
    PlayerManager playerManager;

    void Awake(){
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        playerManager = GetComponent<PlayerManager>();
    }

    // Start is called before the first frame update
    void Start(){
        maxHealth = GetHealthByLevel(level);
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        levelEXP = GetEXPByLevel(level);
        currentEXP = 0;
        expSlider.maxValue = levelEXP;
        expSlider.value = currentEXP;
    }

    float GetHealthByLevel(int currentLevel){
        maxHealth = healthByLevel * 10 * currentLevel;
        return maxHealth;
    }

    int GetEXPByLevel(int currentLevel){
        levelEXP = expByLevel * 10 * currentLevel;
        return levelEXP;
    }

    public void TakeDame(float damage, string getHitAnimation = "Hit"){
        if(isDead) return;

        if(playerManager.isInvulnerable) return;

        currentHealth -= damage;
        
        // Hurt Animation
        animatorHandler.PlayAnimation(getHitAnimation, true);

        // Death
        if(currentHealth <= 0){
            currentHealth = 0;
            
            // Death Animation
            animatorHandler.PlayAnimation("Death", true);
            
            isDead = true;
            // Application.LoadLevel("RestorePoint");
        }
        
        healthSlider.value = currentHealth;
        healthText.text = currentHealth.ToString("F0") + " / " + maxHealth.ToString("F0"); 
    }

    public void HealHP(float health){
        currentHealth += health;
        if(currentHealth > maxHealth) currentHealth = maxHealth;
        healthSlider.value = currentHealth;
        healthText.text = currentHealth.ToString("F0") + " / " + maxHealth.ToString("F0"); 

    }

    public void AddCoins(int coins){
        coin += coins;

        coinText.text = coin.ToString();
    }

    public void AddEXP(int exp){
        currentEXP += exp;

        while(true){
            if(currentEXP < levelEXP) break;

            currentEXP = currentEXP - levelEXP;
            level += 1;
            levelEXP = GetEXPByLevel(level);
        }

        expSlider.maxValue = levelEXP;
        expSlider.value = currentEXP;
    }

}
