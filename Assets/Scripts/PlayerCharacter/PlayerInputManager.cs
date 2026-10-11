using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private Camera PlayerCamera;
    
    private Vector2 CameraRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CameraRotation = new Vector2(0f,0f);
    }

    public void OnLook(InputValue inputVal)
    {
        var input = inputVal.Get<Vector2>();

        CameraRotation.y += input.x;

        CameraRotation.x -= input.y;
        CameraRotation.x = Mathf.Clamp(CameraRotation.x, -70f, 70f);

        PlayerCamera.transform.localRotation = Quaternion.Euler(CameraRotation.x, CameraRotation.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
