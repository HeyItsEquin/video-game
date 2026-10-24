using System.ComponentModel.Design.Serialization;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    private UIDocument Document;
    private VisualElement root;

    [SerializeField] private PlayerCamera playerCamera;

    private Button quitButton;
    private Button resumeButton;
    private SliderInt sensSlider;
    private SliderInt maxFpsSlider;

    private int sensValue = 5;
    private int maxFpsValue = 120;

    private GlobalSounds globalSounds;

    void OnEnable()
    {
        globalSounds = GlobalSounds.GetGlobalSounds();
        Document = GetComponent<UIDocument>();
        root = Document.rootVisualElement;

        sensSlider = root.Q<SliderInt>("Sens");
        sensSlider.value = sensValue;
        sensSlider.RegisterValueChangedCallback(OnSensSliderValueChanged);

        maxFpsSlider = root.Q<SliderInt>("MaxFPS");
        maxFpsSlider.value = maxFpsValue;
        maxFpsSlider.RegisterValueChangedCallback(OnMaxFpsSliderValueChanged);

        resumeButton = root.Q<Button>("RESUME");
        resumeButton.RegisterCallback<ClickEvent>(OnButtonClick);
        resumeButton.RegisterCallback<ClickEvent>(OnResume);
        resumeButton.RegisterCallback<MouseEnterEvent>(OnButtonHover);

        quitButton = root.Q<Button>("QUIT");
        quitButton.RegisterCallback<ClickEvent>(OnQuit);
        quitButton.RegisterCallback<ClickEvent>(OnButtonClick);
        quitButton.RegisterCallback<MouseEnterEvent>(OnButtonHover);
    }

    void Start()
    {
        LoadSettings();
        Hide();
        LoadSettings();
    }

    public void Toggle()
    {
        if (gameObject.activeSelf == true)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void Open()
    {
        LoadSettings();
        gameObject.SetActive(true);
        PauseManager.Pause();
    }

    public void Close()
    {
        SaveSettings();
        gameObject.SetActive(false);
        PauseManager.Unpause();
    }

    void Hide()
    {
        gameObject.SetActive(false);
        PauseManager.Unpause();
    }

    void SaveSettings()
    {
        SaveManager.Instance.Set(Opts.CameraSensitivity, sensValue);

        SaveManager.Instance.Save();
    }

    void LoadSettings()
    {
        sensValue = SaveManager.Instance.Get(Opts.CameraSensitivity);
        playerCamera.SetCameraSensitivity((float)sensValue/10);
    }

    void OnResume(ClickEvent evt)
    {
        Close();
    }

    void OnQuit(ClickEvent evt)
    {
        Application.Quit();
    }

    void OnSensSliderValueChanged(ChangeEvent<int> sens)
    {
        OnButtonHover(null);
        playerCamera.SetCameraSensitivity((float)sens.newValue/10);
        sensValue = sens.newValue;
    }

    void OnMaxFpsSliderValueChanged(ChangeEvent<int> fps)
    {
        OnButtonHover(null);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = fps.newValue;
        maxFpsValue = fps.newValue;
    }

    void OnButtonClick(object evt)
    {
        globalSounds.PlayRandomized(globalSounds.MenuButtonClick, 0.8f, 1.0f);
    }

    void OnButtonHover(object evt)
    {
        globalSounds.PlayRandomized(globalSounds.MenuButtonHover, 0.8f, 1.0f);
    }
}