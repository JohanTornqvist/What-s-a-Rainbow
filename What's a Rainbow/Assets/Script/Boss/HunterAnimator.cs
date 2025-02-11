using UnityEngine;

public class HunterAnimator : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetHunting(bool isHunting)
    {
        animator.SetBool("isHunting", isHunting);
    }

    public void SetJumping(bool isJumping)
    {
        animator.SetBool("isJumpingHunter", isJumping);
    }
}
