using UnityEngine;
using UnityEngine.Assertions;
using RotaryHeart.Lib;
public class MovementManager : MonoBehaviour
{
    public GrabManager PlayerGrabManager;
    [SerializeField] private LayerMask DefaultLMask;
    [SerializeField] private LayerMask GrabMask;
    [SerializeField] private float WalkSpeed = 6f;
    [SerializeField] private float JumpPower = 25f;
    [SerializeField] private float SlopeInfluence = 0.5f;
    [SerializeField] private float SlopeLimit = 0.5f;
    [SerializeField] private float GrabOffSpeed = 10f;

    private Rigidbody PlayerBody;

    private Vector3 MovementDirection;
    private Vector3 InputDirection;

    private RaycastHit groundHit;


    void Start()
    {
        PlayerBody = GetComponent<Rigidbody>();
        Assert.IsNotNull(PlayerBody, "Mreow :3; mlm looking for twink");

        MovementDirection = Vector3.zero;
        InputDirection = Vector3.zero;
    }

    public bool IsGrounded()
    {
        var verticalOffset = Vector3.up/2;

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
            PlayerGrabManager.IsGrabbing = false;
            PlayerBody.linearVelocity = transform.forward * GrabOffSpeed;
            PlayerBody.linearVelocity += Vector3.up * JumpPower;
        }
        if (IsGrounded())
        {
            PlayerBody.linearVelocity += Vector3.up * JumpPower;
        }
        else         {
            Debug.Log("Not grounded");
        }
    }

    

    Vector3 GetLinearVelocity()
    {
        MovementDirection = transform.forward * InputDirection.y;
        MovementDirection += transform.right * InputDirection.x;

        MovementDirection *= WalkSpeed;
        
        if (IsGrounded())
        {
            float slopeMultiplier = Vector3.ProjectOnPlane(MovementDirection, groundHit.normal).normalized.y;
            MovementDirection *= (1 - slopeMultiplier * SlopeInfluence);
        }
        else
        {
            MovementDirection.y = PlayerBody.linearVelocity.y;
        }
        //if (slopeMultiplier > SlopeLimit)
        //{
        //    MovementDirection -= 9.81f * Vector3.up * Time.fixedDeltaTime;
        //    MovementDirection = Vector3.ProjectOnPlane(MovementDirection, groundHit.normal);
        //}

        return MovementDirection;
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
            Vector3 tempReplacePos = transform.position;
            tempReplacePos.y = groundHit.point.y;
            transform.position = tempReplacePos;            
        }
        

        Debug.DrawRay(transform.position, PlayerBody.linearVelocity, Color.green);
    }
}
