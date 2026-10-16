using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private Rigidbody playerBody;

    private MovementManager movementManager;
    private PlayerCamera playerCamera;
    private PlayerInputManager playerInputManager;
    private Checkpoint lastCheckpoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
