using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody platformBody;
    [SerializeField] private float moveSpeed = 3f;

    private Vector3 startPos, endPos, goal;
    public Vector3 Velocity { get; private set; }

    void Start()
    {
        startPos = platformBody.position;
        endPos = target.position;
        goal = endPos;
    }

    void FixedUpdate()
    {
        var current = platformBody.position;
        var next = Vector3.MoveTowards(current, goal, moveSpeed * Time.fixedDeltaTime);

        Velocity = (next - current) / Time.fixedDeltaTime;
        platformBody.MovePosition(next);

        if (next == goal)
            goal = (goal == endPos) ? startPos : endPos;
    }
}