using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private PlayerCamera PlayerCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnLook(InputValue inputVal)
    {
        var input = inputVal.Get<Vector2>();
        PlayerCamera.Rotate(input);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
