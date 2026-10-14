using UnityEngine;
using UnityEngine.Assertions;

public class MovementManager : MonoBehaviour
{

    [SerializeField] private LayerMask DefaultLMask;
    [SerializeField] private float WalkSpeed = 6f;
    [SerializeField] private float JumpPower = 25f;
    [SerializeField] private float SlopeInfluence = 0.5f;
    [SerializeField] private float SlopeLimit = 0.5f;

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

        var ray = Physics.Raycast(
            transform.position + verticalOffset, Vector3.down,
            out groundHit,
            transform.localScale.y*2,
            DefaultLMask);
        
        return ray && PlayerBody.linearVelocity.y <= 0;
    }

    public void Move(Vector3 movementVec)
    {
        InputDirection = movementVec;
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            PlayerBody.linearVelocity += Vector3.up*JumpPower;
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
        MovementDirection.y = PlayerBody.linearVelocity.y;

        float slopeMultiplier = Vector3.ProjectOnPlane(MovementDirection, groundHit.normal).normalized.y;
        MovementDirection *= (1 - slopeMultiplier * SlopeInfluence);
        //if (slopeMultiplier > SlopeLimit)
        //{
        //    MovementDirection -= 9.81f * Vector3.up * Time.fixedDeltaTime;
        //    MovementDirection = Vector3.ProjectOnPlane(MovementDirection, groundHit.normal);
        //}

        return MovementDirection;
    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Vector3.down * transform.localScale.y, Color.red);
        
        PlayerBody.linearVelocity = GetLinearVelocity();

        Debug.DrawRay(transform.position, PlayerBody.linearVelocity, Color.green);
    }
}
