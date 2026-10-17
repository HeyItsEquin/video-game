using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float FieldOfViewWalk;
    [SerializeField] private float FieldOfViewSprint;

    [Range(0, 1.0f)]
    [SerializeField] private float CameraSensitivity = 0.4f;
    [SerializeField] private float MaxGrabTurn = 0.5f;
    [SerializeField] private float ChangeInFovSpeed;

    private MovementManager PlayerMovementManager;

    private float CameraSensitivityFactor;
    private float MinSensitivity = 0.05f;
    private float MaxSensitivity = 2.0f;
    private float FieldOfView;

    private Vector3 CameraRotation;
    private Camera Camera;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        CameraRotation = Vector3.zero;

        Camera = GetComponent<Camera>();
        FieldOfView = Camera.fieldOfView;
        FieldOfViewWalk = FieldOfView;
        PlayerMovementManager = transform.parent.GetComponent<MovementManager>();

        SetCameraSensitivity(CameraSensitivity);
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
    }

    float GrabClamp(float GrabAngle)
    {
        float angleOfGrab = PlayerMovementManager.PlayerGrabManager.grabbedObject.transform.eulerAngles.y - 180f;
        float clampedAngleOfGrab = Mathf.DeltaAngle(angleOfGrab, GrabAngle);
        clampedAngleOfGrab = Mathf.Clamp(clampedAngleOfGrab, -MaxGrabTurn, MaxGrabTurn);
        GrabAngle = angleOfGrab + clampedAngleOfGrab;

        return GrabAngle;
    }

    public void ChangeFovToSprint(bool sprint) 
    {
        FieldOfView = sprint ? FieldOfViewSprint : FieldOfViewWalk;
    }
}
