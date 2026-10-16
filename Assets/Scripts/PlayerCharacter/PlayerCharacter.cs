using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private Checkpoint startingCheckpoint;

    private Rigidbody playerBody;

    private MovementManager movementManager;
    private PlayerCamera playerCamera;
    private PlayerInputManager playerInputManager;

    public Checkpoint lastCheckpoint;

    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        movementManager = GetComponent<MovementManager>();
        playerCamera = GetComponent<PlayerCamera>();
        playerInputManager = GetComponent<PlayerInputManager>();
        //lastCheckpoint = SaveManager.Instance.Get(Opts.PlayerLastCheckpoint);
    }

    void ResetState()
    {
        movementManager.ResetPosition(lastCheckpoint.transform.position);
        playerCamera.SetRotation(Vector3.zero);
    }
}
