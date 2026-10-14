using UnityEngine;
using UnityEngine.UIElements;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private string StartScene;

    private UIDocument Document;
    private VisualElement root;

    private Button quitButton;
    private Button startButton;

    void OnEnable()
    {
        
    }
}