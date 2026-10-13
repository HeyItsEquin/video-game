using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float FieldOfView = 70f;
    
    [Range(0, 1.0f)]
    [SerializeField] private float CameraSensitivity = 0.4f;

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

        SetCameraSensitivity(CameraSensitivity);
    }

    public void Rotate(Vector3 input)
    {
        CameraRotation.y += input.x * CameraSensitivityFactor;

        CameraRotation.x -= input.y * CameraSensitivityFactor;
        CameraRotation.x = Mathf.Clamp(CameraRotation.x, -70f, 70f);

        var normVec = new Vector3(CameraRotation.x, 0, CameraRotation.z);
        transform.localRotation = Quaternion.Euler(normVec);

        transform.parent.localRotation = Quaternion.Euler(new Vector3(0,CameraRotation.y,0));
    }

    public void SetCameraSensitivity(float sens)
    {
        if (sens > 1)
            return;

        CameraSensitivityFactor = Mathf.Lerp(MinSensitivity, MaxSensitivity, sens*sens);
    }

    // Update is called once per frame
    void Update()
    {
        // SetCameraSensitivity(CameraSensitivity);
    }
}
