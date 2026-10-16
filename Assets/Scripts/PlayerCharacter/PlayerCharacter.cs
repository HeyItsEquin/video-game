using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private Rigidbody playerBody;

    private MovementManager movementManager;
    private PlayerCamera playerCamera;
    private PlayerInputManager playerInputManager;
    public Checkpoint lastCheckpoint;
    [SerializeField] private Checkpoint startingCheckpoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
        //lastCheckpoint = SaveManager.Instance.Get(Opts.PlayerLastCheckpoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
