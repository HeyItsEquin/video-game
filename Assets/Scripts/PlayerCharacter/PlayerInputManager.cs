using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private PlayerCamera PlayerCamera;
    [SerializeField] private MovementManager MovementManager;
    private UIScript pauseManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseManager = FindAnyObjectByType<UIScript>();
        pauseManager.playerInputManager = this;
        pauseManager.gameObject.SetActive(false);
    }

    public void OnLook(InputValue inputVal)
    {
        if (Time.timeScale == 0)
            return;
        var input = inputVal.Get<Vector2>();
        PlayerCamera.Rotate(input);
    }

    public void OnMove(InputValue inputVal)
    {
        var input = inputVal.Get<Vector2>();
        MovementManager.Move(input);
    }

    public void OnJump()
    {
        MovementManager.Jump();
    }

    public void OnMenu()
    {
        pauseManager.gameObject.SetActive(!pauseManager.gameObject.activeSelf);
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        Cursor.lockState = Time.timeScale == 0 ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
