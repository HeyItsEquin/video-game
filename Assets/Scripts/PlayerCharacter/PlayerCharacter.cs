using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public PlayerCamera playerCamera;
    [SerializeField] private Checkpoint startingCheckpoint;

    private Rigidbody playerBody;

    private MovementManager movementManager;

    public Checkpoint lastCheckpoint;

    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        movementManager = GetComponent<MovementManager>();
        //lastCheckpoint = SaveManager.Instance.Get(Opts.PlayerLastCheckpoint);
    }

    public void ResetState()
    {
        movementManager.ResetPosition(lastCheckpoint.transform.position);
        playerCamera.SetRotation(Vector3.zero);
    }
}
