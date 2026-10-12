using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float FieldOfView = 70f;
    [SerializeField] private int CameraSensitivity = 4;

    private float CameraSensitivityFactor;
    private float MinSensitivity = 0.05f;
    private float MaxSensitivity = 2.0f;

    private Vector2 CameraRotation;
    private Camera Camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        CameraRotation = Vector2.zero;
        
        Camera = GetComponent<Camera>();
        Camera.fieldOfView = FieldOfView;

        SetCameraSensitivity(CameraSensitivity);
    }

    public void Rotate(Vector2 input)
    {
        CameraRotation.y += input.x * CameraSensitivityFactor;

        CameraRotation.x -= input.y * CameraSensitivityFactor;
        CameraRotation.x = Mathf.Clamp(CameraRotation.x, -70f, 70f);

        transform.localRotation = Quaternion.Euler(CameraRotation.x, CameraRotation.y, 0);
    }

    public void SetCameraSensitivity(int sens)
    {
        if (sens > 10)
            return;

        float t = (float)sens/10;
        CameraSensitivityFactor = Mathf.Lerp(MinSensitivity, MaxSensitivity, t*t);
    }

    // Update is called once per frame
    void Update()
    {
        SetCameraSensitivity(CameraSensitivity);
    }
}
