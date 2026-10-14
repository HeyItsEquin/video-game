using UnityEngine;

public class GrabManager : MonoBehaviour
{

    [SerializeField] private LayerMask GrabMask;
    [SerializeField] private float EasingSpeed = 5f;
    [SerializeField] private Vector3 CameraOffset;

    public bool IsGrabbing = false;
    private MovementManager movementManager;
    private Rigidbody PlayerBody;

    
    private GameObject grabbedObject;
    
    private void Start()
    {
        PlayerBody = GetComponent<Rigidbody>();
        movementManager = GetComponent<MovementManager>();
        
    }
    public void Grab()
    {
        Collider[] grabColliders = Physics.OverlapBox(transform.position + transform.forward + CameraOffset, Vector3.one * 0.5f, Quaternion.identity, GrabMask, QueryTriggerInteraction.Collide);
        RotaryHeart.Lib.PhysicsExtension.DebugExtensions.DebugBox(transform.position + transform.forward + CameraOffset, Vector3.one * 0.5f, Color.red, Quaternion.identity, 0, RotaryHeart.Lib.PhysicsExtension.PreviewCondition.Editor);
        if (!movementManager.IsGrounded() && grabColliders.Length > 0)
        {
            IsGrabbing = true;
            grabbedObject = grabColliders[0].gameObject;
        }
    }

    public void Grabbing()
    {
        PlayerBody.linearVelocity = Vector3.zero;
        if (grabbedObject != null)
        {
            transform.position = Vector3.Lerp(transform.position , grabbedObject.transform.position + grabbedObject.transform.forward - CameraOffset, Time.deltaTime * EasingSpeed);
        }
    }
}
