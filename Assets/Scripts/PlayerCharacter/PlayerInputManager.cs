using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private Camera PlayerCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnLook(InputValue inputVal)
    {
        var input = inputVal.Get<Vector2>();
        var xRotation = Quaternion.AngleAxis(input.x, Vector3.up);
        var yRotation = Quaternion.AngleAxis(-input.y, Vector3.right);

        PlayerCamera.transform.rotation *= xRotation *= yRotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
