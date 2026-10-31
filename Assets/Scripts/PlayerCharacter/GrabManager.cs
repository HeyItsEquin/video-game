using System.Collections;
using UnityEngine;

public class GrabManager : MonoBehaviour
{

    [SerializeField] private LayerMask GrabMask;
    [SerializeField] private float EasingSpeed = 5f;
    [SerializeField] private Vector3 CameraOffset;
    [SerializeField] private float GrabGracePeriod = 0.5f;
    [SerializeField] private float grabOffset;
    public GameObject grabbedObject;

    public float angleOfGrab;
    bool TryingToGrab = false;
    public bool IsGrabbing = false;
    [SerializeField] private Vector3 HangPoint;
    bool isHanging = false;
    private RaycastHit hangHit;
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
            isHanging = false;
            movementManager.playerCharacter.animationManager.animator.SetBool("jumping", true);
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
            angleOfGrab = grabbedObject.transform.eulerAngles.y;
            movementManager.playerCharacter.playerCamera.ChangeFovToSprint(false);
            movementManager.playerCharacter.animationManager.PlayAnimation("Grab");
            movementManager.playerCharacter.animationManager.animator.SetBool("jumping", false);
        }

        
    }

    public void StopGrabbing()
    {
        TryingToGrab = false;
        IsGrabbing = false;
        isHanging = false;
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
            transform.position = Vector3.Lerp(transform.position, grabbedObject.transform.position + grabbedObject.transform.forward * grabOffset - CameraOffset, t);
        }
        else if(isHanging)
        {
            var t = Time.deltaTime * (EasingSpeed * EasingSpeed);
            transform.position = Vector3.Lerp(transform.position, HangPoint - CameraOffset, t);
            if (Physics.Raycast(HangPoint, -hangHit.normal, 1f, movementManager.DefaultLMask))
                HangPoint += Vector3.up * 0.05f;

        }
    }

    public void HangCheck()
    {
        bool wallInfront = Physics.Raycast
            (transform.position + CameraOffset,
            transform.forward, out hangHit,1f, 
            movementManager.DefaultLMask);
        if (wallInfront && PlayerBody.linearVelocity.y < 0
            && !Physics.Raycast(transform.position + Vector3.up*2, 
            transform.forward, 
            1f, movementManager.DefaultLMask) && Mathf.Abs(hangHit.normal.y) < 0.1 &&
            !hangHit.collider.gameObject.CompareTag("MovingPlatform"))
        {
            HangPoint = hangHit.point + hangHit.normal * grabOffset;
            angleOfGrab = Vector3.SignedAngle(Vector3.forward, hangHit.normal, Vector3.up);
            IsGrabbing = true;
            isHanging = true;
            movementManager.playerCharacter.playerCamera.ChangeFovToSprint(false);
            movementManager.playerCharacter.animationManager.PlayAnimation("Grab");
            movementManager.playerCharacter.animationManager.animator.SetBool("jumping", false);
        }
    }

    IEnumerator GrabCourutine()
    {
        TryingToGrab = true;
        yield return new WaitForSeconds(GrabGracePeriod);
        TryingToGrab = false;
    }
}
