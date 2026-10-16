using System.Collections;
using UnityEngine;

public class GrabManager : MonoBehaviour
{

    [SerializeField] private LayerMask GrabMask;
    [SerializeField] private float EasingSpeed = 5f;
    [SerializeField] private Vector3 CameraOffset;
    [SerializeField] private float GrabGracePeriod = 0.5f;
    public GameObject grabbedObject;

    bool TryingToGrab = false;
    public bool IsGrabbing = false;
    private MovementManager movementManager;
    private Rigidbody PlayerBody;

    private void Start()
    {
        PlayerBody = GetComponent<Rigidbody>();
        movementManager = GetComponent<MovementManager>();
        
    }

    private void Update()
    {
        if (TryingToGrab)
        {
            Grab();
        }
    }

    public void Grab()
    {
        if (IsGrabbing)
        {
            IsGrabbing = false;
            return;
        }
        if (!TryingToGrab) 
        { 
            StartCoroutine(GrabCourutine());
        }
        Collider[] grabColliders = Physics.OverlapBox
            (transform.position + transform.forward + CameraOffset,
            Vector3.one * 0.5f, Quaternion.Euler(transform.forward),
            GrabMask, QueryTriggerInteraction.Collide);
        RotaryHeart.Lib.PhysicsExtension.DebugExtensions.DebugBox(transform.position + transform.forward + CameraOffset, Vector3.one * 0.5f, Color.red, Quaternion.Euler(transform.forward), 0, RotaryHeart.Lib.PhysicsExtension.PreviewCondition.Editor);
        if (!movementManager.IsGrounded() && grabColliders.Length > 0)
        {
            IsGrabbing = true;
            grabbedObject = grabColliders[0].gameObject;
            TryingToGrab = false;
        }
    }

    public void StopGrabbing()
    {
        TryingToGrab = false;
        IsGrabbing = false;
        grabbedObject = null;
        StopCoroutine(GrabCourutine()); // Just incase the grab grace-period is still active
    }

    public void Grabbing()
    {
        PlayerBody.linearVelocity = Vector3.zero;
        if (grabbedObject != null)
        {
            // Quadratic curve for move speed
            var t = Time.deltaTime * (EasingSpeed*EasingSpeed);
            transform.position = Vector3.Lerp(transform.position, grabbedObject.transform.position + grabbedObject.transform.forward - CameraOffset, t);
        }
    }

    IEnumerator GrabCourutine()
    {
        TryingToGrab = true;
        yield return new WaitForSeconds(GrabGracePeriod);
        TryingToGrab = false;
    }
}
