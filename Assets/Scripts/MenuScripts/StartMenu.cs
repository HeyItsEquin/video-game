using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private string StartScene;

    private UIDocument Document;
    private VisualElement root;

    private Button quitButton;
    private Button startButton;

    private GlobalSounds globalSounds;

    void OnEnable()
    {
        globalSounds = GlobalSounds.GetGlobalSounds();
        Document = GetComponent<UIDocument>();
        root = Document.rootVisualElement;

        startButton = root.Q<Button>("START");
        startButton.RegisterCallback<ClickEvent>(OnStartClicked);
        startButton.RegisterCallback<ClickEvent>(OnButtonClick);
        startButton.RegisterCallback<MouseEnterEvent>(OnButtonHover);

        quitButton = root.Q<Button>("QUIT");
        quitButton.RegisterCallback<ClickEvent>(OnQuitClicked);
        quitButton.RegisterCallback<ClickEvent>(OnButtonClick);
        quitButton.RegisterCallback<MouseEnterEvent>(OnButtonHover);
    }

    void OnStartClicked(ClickEvent evt)
    {
        StartCoroutine(LoadSceneTransition());
    }

    IEnumerator LoadSceneTransition()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(StartScene);
    }

    void OnQuitClicked(ClickEvent evt)
    {
        Application.Quit();
    }

    void OnButtonHover(MouseEnterEvent evt)
    {
        globalSounds.PlayRandomized(globalSounds.MenuButtonHover, 0.8f, 1.0f);
    }
    void OnButtonClick(ClickEvent evt)
    {
        globalSounds.PlayRandomized(globalSounds.MenuButtonClick, 0.8f, 1.0f);
    }
}