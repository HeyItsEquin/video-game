using UnityEngine;
using UnityEngine.Assertions;

public class MovementManager : MonoBehaviour
{

    [SerializeField] private LayerMask DefaultLMask;
    [SerializeField] private float WalkSpeed = 6f;
    [SerializeField] private float JumpPower = 25f;

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
        var ray = Physics.Raycast(
            transform.position, Vector3.down,
            out groundHit,
            transform.localScale.y,
            DefaultLMask);
        return ray;
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
    }

    Vector3 GetLinearVelocity()
    {
        MovementDirection = transform.forward * InputDirection.y;
        MovementDirection += transform.right * InputDirection.x;

        MovementDirection *= WalkSpeed;
        MovementDirection.y = PlayerBody.linearVelocity.y;

        return MovementDirection;
    }

    void FixedUpdate()
    {
        PlayerBody.linearVelocity = GetLinearVelocity();
    }
}
