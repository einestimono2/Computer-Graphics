using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public int currentWeaponDamage = 25;

    Collider damageCollider;

    void Awake(){
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
    }

    public void EnableCollider(){
        damageCollider.enabled = true;
    }

    public void DisableCollider(){
        damageCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            BlockingCollider shield = other.transform.GetComponentInChildren<BlockingCollider>();
            PlayerManager playerManager = other.GetComponentInChildren<PlayerManager>();
            CharacterEffects playerEffects = other.GetComponentInChildren<CharacterEffects>();

            if(playerManager != null){
                if(playerManager.isParrying){
                    // Quái bị choáng trong x (s)
                    // Không nhận sát thương
                    return;
                }

                if(shield != null && playerManager.isBlocking){
                    float damage = currentWeaponDamage - (currentWeaponDamage * shield.blockingDamageAbsorption / 100);
                
                    if(playerStats != null){
                        playerStats.TakeDame(damage, "Block Guard");
                        return;
                    }
                }
            }

            if(playerStats != null){
                Vector3 contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                playerEffects.PlayBloodSplatterFX(contactPoint);

                playerStats.TakeDame(currentWeaponDamage);
            }
        }

        if(other.tag == "Enemy"){
            EnemyStats enemyStats = other.GetComponentInParent<EnemyStats>();
            CharacterEffects enemyEffects = other.GetComponent<CharacterEffects>();
            
            if(enemyStats != null){
                Vector3 contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                enemyEffects.PlayBloodSplatterFX(contactPoint);

                enemyStats.TakeDame(currentWeaponDamage);
            }
        }
    }
}
