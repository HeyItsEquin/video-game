using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator animator;
    [SerializeField] SkinnedMeshRenderer meshRenderer;
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

    public void Rendering(bool render)
    {
        meshRenderer.enabled = render;
    }
}
