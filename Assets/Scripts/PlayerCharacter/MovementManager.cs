using UnityEngine;
using UnityEngine.Assertions;
using System;
public class MovementManager : MonoBehaviour
{
    public GrabManager PlayerGrabManager;
    public PlayerCharacter playerCharacter;
    private GlobalSounds globalSounds;
    [SerializeField] private LayerMask DefaultLMask;
    [SerializeField] private LayerMask GrabMask;
    [SerializeField] private float WalkSpeed = 4f;
    [SerializeField] private float RunSpeed = 10f;
    [SerializeField] private float JumpPower = 25f;
    [SerializeField] private float SlopeInfluence = 0.5f;
    [SerializeField] private float SlopeLimit = 0.5f;
    [SerializeField] private float GrabOffSpeed = 10f;

    private Rigidbody PlayerBody;

    private Vector3 MovementDirection = Vector3.zero;
    private Vector3 InputDirection = Vector3.zero;

    private RaycastHit groundHit;
    private float MoveSpeed;


    void Start()
    {
        PlayerBody = GetComponent<Rigidbody>();
        playerCharacter = GetComponent<PlayerCharacter>();
        globalSounds = GlobalSounds.GetGlobalSounds();
        Assert.IsNotNull(PlayerBody, "Mreow :3; mlm looking for twink");

        

        MovementDirection = Vector3.zero;
        InputDirection = Vector3.zero;
        MoveSpeed = WalkSpeed;

    }

    public bool IsGrounded()
    {
        var verticalOffset = Vector3.up/2;

        // checks if the player is grounded by casting a box downwards from the player postion + a camera offset
        var ray = Physics.BoxCast(
            transform.position + verticalOffset,
            Vector3.one/2 - Vector3.up*0.4f,
            Vector3.down,
            out groundHit,
            Quaternion.Euler(transform.forward),
            transform.localScale.y/1.9f,
            DefaultLMask);
        
        return ray && PlayerBody.linearVelocity.y <= 0;
    }

    public void Move(Vector3 movementVec)
    {
        InputDirection = movementVec;
    }

    public void Jump()
    {
        if (PlayerGrabManager.IsGrabbing)
        {
            // sets the player to not grabbing and jumps off the object they are grabbing
            PlayerGrabManager.IsGrabbing = false;
            PlayerBody.linearVelocity += Vector3.up * JumpPower;
            globalSounds.PlayRandomized(globalSounds.jumpSound, 0.9f, 1);
        }
        
        if (IsGrounded())
        {
            PlayerBody.linearVelocity += Vector3.up * JumpPower;
            globalSounds.PlayRandomized(globalSounds.jumpSound, 0.9f, 1);
        }
        else 
        {
            Debug.Log("Not grounded");
        }
    }

    Vector3 GetLinearVelocity()
    {
        MovementDirection = transform.forward * InputDirection.y;
        MovementDirection += transform.right * InputDirection.x;


        MovementDirection *= MoveSpeed;

        
        MovementDirection *= WalkSpeed;

        if (IsGrounded())
        {
            // if player is grounded use slopes to influence the movement direction
            float slopeMultiplier = Vector3.ProjectOnPlane(MovementDirection, groundHit.normal).normalized.y;
            MovementDirection *= (1 - slopeMultiplier * SlopeInfluence);
        }
        else
        {
            MovementDirection.y = PlayerBody.linearVelocity.y;
        }

        return MovementDirection;
    }

    // "Hoist" player up if their feet are colliding with an object
    void VerticalAlignPlayer()
    {
        Vector3 temp = transform.position;
        temp.y = groundHit.point.y;
        transform.position = temp; // Should probably find a better solution than setting transform.position, later.
    }

    public void ResetMovement()
    {
        PlayerGrabManager.StopGrabbing();
        PlayerBody.linearVelocity = Vector3.zero;
    }

    public void ResetPosition(Vector3 pos)
    {
        ResetMovement();
        PlayerBody.transform.position = pos;
    }

    void FixedUpdate()
    {
        RotaryHeart.Lib.PhysicsExtension.Physics.BoxCast(transform.position + Vector3.up/2, Vector3.one / 2 - Vector3.up * 0.4f, Vector3.down, Quaternion.Euler(transform.forward), transform.localScale.y/1.9f, RotaryHeart.Lib.PhysicsExtension.PreviewCondition.Editor);
        
        if (PlayerGrabManager.IsGrabbing)
            PlayerGrabManager.Grabbing();
        else
            PlayerBody.linearVelocity = GetLinearVelocity();

        if (IsGrounded())
        {
            VerticalAlignPlayer();
        }
        

        Debug.DrawRay(transform.position, PlayerBody.linearVelocity, Color.green);
    }

    public void Sprint(float pushed)
    {
        // Checks if the amount pushed down is enough to sprint
        // if true sprint else walk
        MoveSpeed = pushed > 0.5 ? RunSpeed : WalkSpeed;
        if (PlayerGrabManager.IsGrabbing)
            return;
        playerCharacter.playerCamera.ChangeFovToSprint(MoveSpeed == RunSpeed);
        
    }
}
