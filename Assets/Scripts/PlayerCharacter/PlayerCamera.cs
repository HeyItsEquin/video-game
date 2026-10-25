using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float FieldOfViewWalk;
    [SerializeField] private float FieldOfViewSprint;

    [Range(0, 110.0f)]
    [SerializeField] private float MaxGrabTurn = 0.5f;
    [SerializeField] private float ChangeInFovSpeed;
    [SerializeField] private float stepFrequency;
    [SerializeField] private float stepAmount;

    private MovementManager PlayerMovementManager;
    private PlayerCharacter playerCharacter;
    private Rigidbody playerRB;

    private float CameraSensitivityFactor = 0.5f;
    private float MinSensitivity = 0.05f;
    private float MaxSensitivity = 2.0f;
    private float FieldOfView;

    private Vector3 CameraRotation;
    private Camera Camera;
    private GameObject cameraObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        CameraRotation = Vector3.zero;

        cameraObject = transform.GetChild(0).gameObject;
        Camera = cameraObject.GetComponent<Camera>();
        FieldOfView = Camera.fieldOfView;
        FieldOfViewWalk = FieldOfView;
        PlayerMovementManager = transform.parent.GetComponent<MovementManager>();
        playerCharacter = transform.parent.GetComponent<PlayerCharacter>();
        playerRB = PlayerMovementManager.GetComponent<Rigidbody>();
    }

    public void Rotate(Vector3 input)
    {
        // Process mouse deltas
        CameraRotation.y += input.x * CameraSensitivityFactor;

        CameraRotation.x -= input.y * CameraSensitivityFactor;
        CameraRotation.x = Mathf.Clamp(CameraRotation.x, -70f, 70f);

        if (PlayerMovementManager.PlayerGrabManager.IsGrabbing)
        {
            // Camera shouldn't be able to rotate full 180deg when grabbing
            CameraRotation.y = GrabClamp(CameraRotation.y);
        }

        ApplyRotation();
    }

    private void ApplyRotation()
    {
        var normalVec = new Vector3(CameraRotation.x, 0, CameraRotation.z);
        var playerRotationVec = new Vector3(0, CameraRotation.y, 0);

        transform.localRotation = Quaternion.Euler(normalVec);

        // Rotation about the Y-axis should rotate player, not camera
        transform.parent.localRotation = Quaternion.Euler(playerRotationVec);
    }

    public void SetRotation(Vector3 euler)
    {
        CameraRotation = euler;
        ApplyRotation();
    }

    public void SetCameraSensitivity(float sens)
    {
        if (sens > 1)
            return;

        CameraSensitivityFactor = Mathf.Lerp(MinSensitivity, MaxSensitivity, sens * sens);
    }

    // Update is called once per frame
    void Update()
    {
        // SetCameraSensitivity(CameraSensitivity);
        Camera.fieldOfView = Mathf.Lerp(Camera.fieldOfView, FieldOfView, Time.deltaTime * ChangeInFovSpeed);
        // CameraBob();
    }

    float GrabClamp(float GrabAngle)
    {
        float angleOfGrab = PlayerMovementManager.PlayerGrabManager.grabbedObject.transform.eulerAngles.y - 180f;
        float clampedAngleOfGrab = Mathf.DeltaAngle(angleOfGrab, GrabAngle);
        playerCharacter.animationManager.animator.SetFloat("grabAngle", clampedAngleOfGrab);
        clampedAngleOfGrab = Mathf.Clamp(clampedAngleOfGrab, -MaxGrabTurn, MaxGrabTurn);
        GrabAngle = angleOfGrab + clampedAngleOfGrab;

        return GrabAngle;
    }

    public void ChangeFovToSprint(bool sprint) 
    {
        FieldOfView = sprint ? FieldOfViewSprint : FieldOfViewWalk;
    }

    void CameraBob()
    {
        Vector3 velocity = playerRB.linearVelocity * Time.deltaTime;
        if (PlayerMovementManager.IsGrounded() || true)
        {
            velocity.y = 0f;
            cameraObject.transform.localPosition = StepCamera(velocity.magnitude);
        }
        //velocity *= cameraChangeAmplitude;
        //cameraOldVel = Vector3.Lerp(cameraOldVel, velocity, Time.deltaTime * catchUpSpeed);

        //var newChangeInPos = velocity - cameraOldVel;
        //newChangeInPos.y += 1.5f;
        //transform.position = transform.parent.position + newChangeInPos;


    }

    Vector3 StepCamera(float vel)
    {
        Vector3 pos = Vector3.zero;
        pos.x += Mathf.Sin(Time.time * stepFrequency) * stepAmount;
        pos.y += Mathf.Cos(Time.time * stepFrequency * 2) * stepAmount * 2;
        pos *= vel;

        Vector3 rotation = cameraObject.transform.localEulerAngles;
        rotation.z = Mathf.Sin(Time.time * stepFrequency + 0.5f) * stepAmount;
        cameraObject.transform.localEulerAngles = rotation;
        return pos;
    }
}