using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    Animator animator;
    MovementManager movementManager;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        movementManager = GetComponent<MovementManager>();

    }

    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
}
