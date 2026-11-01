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
        meshRenderer.enabled = SaveManager.Instance.Get(Opts.ShowHands);
    }

    

    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }

    public void Rendering(bool render)
    {
        
    }
}
