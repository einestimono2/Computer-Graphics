using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : CharacterStats
{
    EnemyAnimator enemyAnimator;

    [Header("UI")]
    public Slider healthSlider;

    [Header("Prize")]
    public int coinBonus = 10;
    public int expBonus = 100;

    void Awake(){
        enemyAnimator = GetComponentInChildren<EnemyAnimator>();
    }

    // Start is called before the first frame update
    void Start(){
        maxHealth = GetHealthByLevel();
        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update(){
        if(currentHealth == maxHealth || currentHealth == 0f){
            healthSlider.gameObject.SetActive(false);
        }else{
            healthSlider.gameObject.SetActive(true);
        }
    }

    float GetHealthByLevel(){
        maxHealth = healthByLevel * 10;
        return maxHealth;
    }

    public void TakeDame(float damage){
        if(isDead) return;

        currentHealth -= damage;
        
        // Animation TakeDamem
        enemyAnimator.PlayAnimation("Hit1", true);

        // Death
        if(currentHealth <= 0){
            HandleDeath();
        }

        healthSlider.value = currentHealth;
    }

    void HandleDeath(){
        currentHealth = 0;

        // Animation Death
        enemyAnimator.PlayAnimation("Fall1", true);

        isDead = true;

        // Tiền + Điểm kinh nghiệm
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        if(playerStats != null){
            playerStats.AddCoins(coinBonus);
            playerStats.AddEXP(expBonus);
        }

        StartCoroutine(DestroyEnemy());
    }

    IEnumerator DestroyEnemy(){
        yield return new WaitForSeconds(1.5f);

        Destroy(gameObject);
    }

}
