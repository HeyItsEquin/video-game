using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject platform;

    private Vector3 targetPosition;

    private float moveSpeed = 3.0f;

    void Ready()
    {
        targetPosition = target.transform.localPosition;
    }

    Vector3 GetTargetPosition()
    {
        if (targetPosition == Vector3.zero)
        {
            return target.transform.localPosition;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 GetVelocityVector()
    {
        if (targetPosition == target.transform.localPosition)
        {
            return Vector3.Normalize(target.transform.localPosition) * moveSpeed;
        }
        return Vector3.Normalize(-target.transform.localPosition) * moveSpeed;
    }

    void Update()
    {
        float step = moveSpeed * Time.deltaTime;
        
        platform.transform.localPosition = Vector3.MoveTowards(platform.transform.localPosition, targetPosition, step);

        if (Vector3.Distance(platform.transform.localPosition, targetPosition) < 0.01f)
        {
            targetPosition = GetTargetPosition();
        }
    }
}