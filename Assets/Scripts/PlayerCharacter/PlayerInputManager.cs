using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private PlayerCamera PlayerCamera;
    [SerializeField] private PauseMenu pauseMenu;

    private PlayerCharacter playerCharacter;
    private MovementManager MovementManager;
    private GlobalSounds globalSounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCharacter = GetComponent<PlayerCharacter>();
        MovementManager = GetComponent<MovementManager>();
    }

    public void OnLook(InputValue inputVal)
    {
        if (PauseManager.IsPaused()) return;

        var input = inputVal.Get<Vector2>();
        PlayerCamera.Rotate(input);
    }

    public void OnMove(InputValue inputVal)
    {
        var input = inputVal.Get<Vector2>();
        MovementManager.Move(input);
        MovementManager.Sprint(GetComponent<PlayerInput>().actions.FindAction("Sprint").ReadValue<float>());
    }

    public void OnJump()
    {
        MovementManager.Jump();
    }

    public void OnMenu()
    {
        pauseMenu.Toggle();
    }

    public void OnGrab()
    {
        MovementManager.PlayerGrabManager.Grab();
        
    }

    public void OnSuicide()
    {
        playerCharacter.ResetState();
    }

    public void OnSprint(InputValue pushed)
    {
        MovementManager.Sprint(pushed.Get<float>());
    }
    void Update()
    {
        
    }
}
