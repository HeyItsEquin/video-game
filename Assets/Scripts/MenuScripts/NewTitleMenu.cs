using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class NewTitleMenu : MonoBehaviour
{
    [SerializeField] private string startScene;
    [SerializeField] private OptionsMenu optionsMenu;

    private UIDocument document;
    private VisualElement root;

    // Menu buttons
    private Button startButton;
    private Button optionsButton;
    private Button exitButton;

    void Start()
    {
        document = GetComponent<UIDocument>();
    }

    void OnEnable()
    {
        root = document.rootVisualElement;
        
        startButton = root.Q<Button>("StartButton");
        startButton.clicked += OnStartClicked;

        optionsButton = root.Q<Button>("OptionsButton");
        optionsButton.clicked += OnOptionsClicked;

        exitButton = root.Q<Button>("ExitButton");
        exitButton.clicked += OnExitClicked;
    }

    void OnStartClicked()
    {
        StartCoroutine(LoadSceneTransition());
    }

    void OnOptionsClicked()
    {
        
    }

    void OnExitClicked()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    IEnumerator LoadSceneTransition()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(startScene);
    }

    void Update()
    {
        
    }
}