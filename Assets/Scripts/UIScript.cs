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
    private Button resumeButton;
    private Button quitButton;
    private Slider sensSlider;
    [SerializeField] private AudioSource hoverSFX;
    [SerializeField] private AudioSource clickSFX;
    public PlayerInputManager playerInputManager;
    private PlayerCamera playerCamera;
    private float sensValue = 0.5f;
    private void OnEnable()
    {
        playerCamera = FindAnyObjectByType<PlayerCamera>();
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        startButton = root.Q<Button>("START");
        if (startButton == null)
        {
            resumeButton = root.Q<Button>("RESUME");
            resumeButton.clicked += () => ReturnBack();
            resumeButton.RegisterCallback<MouseEnterEvent>(evt => PlaySFXUsingRandom(hoverSFX));

            sensSlider = root.Q<Slider>("Sens");
            sensSlider.value = sensValue;
            sensSlider.RegisterValueChangedCallback(ChangePlayerSens);

        }
        else
        {
            startButton.clicked += () => StartCoroutine(LoadSceneTransition());
            startButton.RegisterCallback<MouseEnterEvent>(evt => PlaySFXUsingRandom(hoverSFX));
        }
        quitButton = root.Q<Button>("QUIT");


        quitButton.clicked += () => Application.Quit();
        quitButton.RegisterCallback<MouseEnterEvent>(evt => PlaySFXUsingRandom(hoverSFX));
    }
    private void ReturnBack()
    {
        playerInputManager.OnMenu();
        PlaySFXUsingRandom(clickSFX);
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
    private void ChangePlayerSens(ChangeEvent<float> sens)
    {
        playerCamera.SetCameraSensitivity(sens.newValue);
        PlaySFXUsingRandom(hoverSFX);
        sensValue = sens.newValue;

    }
}
