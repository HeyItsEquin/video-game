using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIScript : MonoBehaviour
{
    private UIDocument uiDocument;
    [SerializeField] private string startButtonScene;
    private VisualElement root;
    private Button startButton;
    private Button quitButton;
    [SerializeField] private AudioSource hoverSFX;
    [SerializeField] private AudioSource clickSFX;
    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        startButton = root.Q<Button>("START");
        quitButton = root.Q<Button>("QUIT");

        startButton.clicked += () => StartCoroutine(LoadSceneTransition());
        startButton.RegisterCallback<MouseEnterEvent>(evt => PlaySFXUsingRandom(hoverSFX));
        quitButton.clicked += () => Application.Quit();
        quitButton.RegisterCallback<MouseEnterEvent>(evt => PlaySFXUsingRandom(hoverSFX));
    }

    private IEnumerator LoadSceneTransition()
    {
        PlaySFXUsingRandom(clickSFX);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(startButtonScene);
    }
    public static void PlaySFXUsingRandom(AudioSource audioSource)
    {
        if (audioSource != null)
        {
            audioSource.pitch = Random.Range(0.8f, 1.0f);
            audioSource.volume = Random.Range(0.8f, 1f);
            audioSource.Play();
        }
    }

}
