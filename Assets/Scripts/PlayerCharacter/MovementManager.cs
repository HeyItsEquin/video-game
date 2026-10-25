using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using PhysExt = RotaryHeart.Lib.PhysicsExtension;

public class MovementManager : MonoBehaviour
{
    public GrabManager PlayerGrabManager;
    public PlayerCharacter playerCharacter;
    private GlobalSounds globalSounds;

    [SerializeField] private CapsuleCollider capsule;
    private float groundSnapY;

    [SerializeField] private LayerMask DefaultLMask;

    [SerializeField] private float WalkSpeed = 4f;
    [SerializeField] private float RunSpeed = 10f;
    [SerializeField] private float JumpHeight = 1.5f;
    private float JumpSpeed => Mathf.Sqrt(2f * -Physics.gravity.y * JumpHeight);
    
    [SerializeField] private float SlopeInfluence = 0.5f;
    [SerializeField] private float MaxSlopeAngle = 45;

    private Rigidbody PlayerBody;

    private Vector3 InputDirection = Vector3.zero;

    private RaycastHit groundHit;
    private float MoveSpeed;

    private bool grounded;
    private Vector3 groundVelocity;
    private Vector3 appliedGroundVelocity;

    private bool shouldSprint;

    public bool IsGrounded() => grounded;


    void Start()
    {
        PlayerBody = GetComponent<Rigidbody>();
        playerCharacter = GetComponent<PlayerCharacter>();
        globalSounds = GlobalSounds.GetGlobalSounds();
        Assert.IsNotNull(PlayerBody, "Mreow :3; mlm looking for twink");

        InputDirection = Vector3.zero;
        MoveSpeed = WalkSpeed;
    }

    void UpdateGroundedState()
    {
        groundVelocity = Vector3.zero;

        float r = capsule.radius * 0.95f;
        Vector3 origin = PlayerBody.position + Vector3.up * (r + 0.1f);
        float castDistance = 0.2f;

        bool hit = Physics.SphereCast(origin, r, Vector3.down, out groundHit, castDistance, DefaultLMask);

        #if UNITY_EDITOR
        PhysExt.Physics.SphereCast(origin, r, Vector3.down, castDistance, PhysExt.PreviewCondition.Editor);
        #endif

        if (hit)
            groundSnapY = origin.y - groundHit.distance - r;

        if (hit && groundHit.collider.CompareTag("MovingPlatform"))
        {
            var platform = groundHit.collider.GetComponentInParent<MovingPlatform>();
            if (platform != null) groundVelocity = platform.Velocity;
        }

        // Judge vertical movement relative to the ground, if the ground is moving, the player should not be considered "rising"
        Vector3 relativeVel = PlayerBody.linearVelocity - appliedGroundVelocity;
        float awayFromGround = hit ? Vector3.Dot(relativeVel, groundHit.normal) : 0f;

        float minNormalY = Mathf.Cos(MaxSlopeAngle * Mathf.Deg2Rad);
        grounded = hit
            && groundHit.distance > 0f
            && groundHit.normal.y >= minNormalY
            && awayFromGround < JumpSpeed * 0.25f;
    }

    public void Move(Vector3 movementVec)
    {
        InputDirection = movementVec;
        shouldSprint = !(movementVec == Vector3.zero);
    }

    public void Jump()
    {
        if (PlayerGrabManager.IsGrabbing)
        {
            PlayerGrabManager.IsGrabbing = false;
            ApplyJump(0f);
            playerCharacter.animationManager.PlayAnimation("Jump");
            playerCharacter.animationManager.animator.SetBool("jumping", true);
        }
        else if (IsGrounded())
        {
            ApplyJump(groundVelocity.y);
            playerCharacter.animationManager.PlayAnimation("Jump");
            playerCharacter.animationManager.animator.SetBool("jumping", true);
        }
        else
            Debug.Log("Not grounded");
    }

    void ApplyJump(float baseVelocityY)
    {
        var v = PlayerBody.linearVelocity;
        v.y = baseVelocityY + JumpSpeed;
        PlayerBody.linearVelocity = v;
        globalSounds.PlayRandomized(globalSounds.jumpSound, 0.9f, 1);
    }

    Vector3 GetLinearVelocity()
    {
        Vector3 move = (transform.forward * InputDirection.y + transform.right * InputDirection.x) * MoveSpeed;

        if (grounded)
        {
            Vector3 along = Vector3.ProjectOnPlane(move, groundHit.normal).normalized;
            move = along * move.magnitude * (1 - along.y * SlopeInfluence);
            appliedGroundVelocity = groundVelocity;
            return move + groundVelocity;
        }

        appliedGroundVelocity = Vector3.zero;
        move.y = PlayerBody.linearVelocity.y;
        return move;
    }

    // "Hoist" player up if their feet are colliding with an object
    void VerticalAlignPlayer()
    {
        if (groundHit.normal.y < 0.99f) return;
        if (groundHit.distance <= 0f) return;
        if (Mathf.Abs(groundSnapY - PlayerBody.position.y) > 0.15f) return;

        Vector3 p = PlayerBody.position;
        p.y = groundSnapY;
        PlayerBody.position = p;
        playerCharacter.animationManager.animator.SetBool("jumping", false);
    }

    public void ResetMovement()
    {
        PlayerGrabManager.StopGrabbing();
        PlayerBody.linearVelocity = Vector3.zero;
    }

    public void ResetPosition(Vector3 pos)
    {
        ResetMovement();
        PlayerBody.position = pos;
    }

    public void Sprint(float pushed)
    {
        // Checks if the amount pushed down is enough to sprint
        // if true sprint else walk
        MoveSpeed = pushed > 0.5 ? RunSpeed : WalkSpeed;
        
        if (PlayerGrabManager.IsGrabbing)
            return;
        playerCharacter.playerCamera.ChangeFovToSprint(MoveSpeed == RunSpeed && shouldSprint);
        playerCharacter.animationManager.animator.SetFloat("velocity", MoveSpeed * InputDirection.magnitude / RunSpeed);
    }

    void FixedUpdate()
    {
        UpdateGroundedState();

        if (PlayerGrabManager.IsGrabbing)
            PlayerGrabManager.Grabbing();
        else
            PlayerBody.linearVelocity = GetLinearVelocity();

        if (grounded)
            VerticalAlignPlayer();
    }
}
