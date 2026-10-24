using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public PlayerCamera playerCamera;
    public AnimationManager animationManager;
    [SerializeField] private Checkpoint startingCheckpoint;

    private Rigidbody playerBody;

    private MovementManager movementManager;

    public Checkpoint lastCheckpoint;

    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        movementManager = GetComponent<MovementManager>();
        animationManager = GetComponent<AnimationManager>();
        //lastCheckpoint = SaveManager.Instance.Get(Opts.PlayerLastCheckpoint);
    }

    public void ResetState()
    {
        movementManager.ResetPosition(lastCheckpoint.transform.position);
        playerCamera.SetRotation(Vector3.zero);
    }
}
