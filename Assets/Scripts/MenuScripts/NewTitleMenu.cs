using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class NewTitleMenu : MenuScript
{
    [SerializeField] private string startScene;
    [SerializeField] private OptionsMenu optionsMenu;
    [SerializeField] private CreditsMenu creditsMenu;

    private Button startButton;
    private Button optionsButton;
    private Button exitButton;
    private Button creditsButton;

    protected override void OnRootReady()
    {
        startButton = root.Q<Button>("StartButton");
        startButton.clicked += OnStartClicked;

        optionsButton = root.Q<Button>("OptionsButton");
        optionsButton.clicked += OnOptionsClicked;

        exitButton = root.Q<Button>("ExitButton");
        exitButton.clicked += OnExitClicked;

        creditsButton = root.Q<Button>("CreditsButton");
        creditsButton.clicked += OnCreditsClicked;
    }

    void OnStartClicked()
    {
        StartCoroutine(LoadSceneTransition());
    }

    void OnOptionsClicked()
    {
        optionsMenu.Open(this);
        Close();
    }

    void OnExitClicked()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    void OnCreditsClicked()
    {
        creditsMenu.Open(this);
        Close();
    }

    IEnumerator LoadSceneTransition()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(startScene);
    }
}