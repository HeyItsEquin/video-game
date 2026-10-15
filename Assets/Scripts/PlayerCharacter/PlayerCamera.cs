using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float FieldOfView = 70f;

    [Range(0, 1.0f)]
    [SerializeField] private float CameraSensitivity = 0.4f;
    [SerializeField] private float MaxGrabTurn = 0.5f;

    private MovementManager PlayerMovementManager;

    private float CameraSensitivityFactor;
    private float MinSensitivity = 0.05f;
    private float MaxSensitivity = 2.0f;

    private Vector3 CameraRotation;
    private Camera Camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        CameraRotation = Vector3.zero;

        Camera = GetComponent<Camera>();
        Camera.fieldOfView = FieldOfView;
        PlayerMovementManager = transform.parent.GetComponent<MovementManager>();

        SetCameraSensitivity(CameraSensitivity);
    }

    public void Rotate(Vector3 input)
    {
        CameraRotation.y += input.x * CameraSensitivityFactor;

        CameraRotation.x -= input.y * CameraSensitivityFactor;
        CameraRotation.x = Mathf.Clamp(CameraRotation.x, -70f, 70f);

        var normVec = new Vector3(CameraRotation.x, 0, CameraRotation.z);
        if (PlayerMovementManager.PlayerGrabManager.IsGrabbing)
        {
            CameraRotation.y = GrabClamp(CameraRotation.y);
        }    

        transform.localRotation = Quaternion.Euler(normVec);

        transform.parent.localRotation = Quaternion.Euler(new Vector3(0, CameraRotation.y, 0));
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
    }

    float GrabClamp(float GrabAngle)
    {
        
        float angleOfGrab = PlayerMovementManager.PlayerGrabManager.grabbedObject.transform.eulerAngles.y - 180f;
        float clampedAngleOfGrab = Mathf.DeltaAngle(angleOfGrab, GrabAngle);
        clampedAngleOfGrab = Mathf.Clamp(clampedAngleOfGrab, -MaxGrabTurn, MaxGrabTurn);
        GrabAngle = angleOfGrab + clampedAngleOfGrab;

        
        return GrabAngle;
    }
}
