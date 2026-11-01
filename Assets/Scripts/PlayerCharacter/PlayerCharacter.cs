using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public PlayerCamera playerCamera;
    public AnimationManager animationManager;
    [SerializeField] private Checkpoint startingCheckpoint;

    private Rigidbody playerBody;

    private MovementManager movementManager;

    public Checkpoint lastCheckpoint;

    private GlobalSounds globalSounds;

    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        movementManager = GetComponent<MovementManager>();
        animationManager = GetComponent<AnimationManager>();
        //lastCheckpoint = SaveManager.Instance.Get(Opts.PlayerLastCheckpoint);

        globalSounds = GlobalSounds.GetGlobalSounds();
        globalSounds.PlaySound(globalSounds.AmbientWind, volume: 0.5f, loop: true);

        movementManager.HitGround += OnHitGround;
    }

    void OnHitGround(RaycastHit hitObj)
    {
        if (hitObj.collider.CompareTag("KillPlane"))
            Die();
    }

    public void Die()
    {
        ResetState();
    }

    public void ResetState()
    {
        movementManager.ResetPosition(lastCheckpoint.transform.position);
        playerCamera.SetRotation(Vector3.zero);
    }
}
