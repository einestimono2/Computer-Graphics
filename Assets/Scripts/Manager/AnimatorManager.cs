using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator anim;

    public bool canRotate;
    
    public void PlayAnimation(string targetAnimation, bool isInteracting, bool canRotate = false){
        anim.applyRootMotion = isInteracting;
        anim.SetBool("canRotate", canRotate);
        anim.SetBool("isInteracting", isInteracting);
        anim.CrossFade(targetAnimation, 0.1f);
    }
}
