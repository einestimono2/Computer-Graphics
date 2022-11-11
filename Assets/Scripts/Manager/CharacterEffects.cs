using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    public GameObject bloodSplatterFX;

    public virtual void PlayBloodSplatterFX(Vector3 location){
        GameObject blood = Instantiate(bloodSplatterFX, location, Quaternion.identity);
    }
}
