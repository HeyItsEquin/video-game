using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : MenuScript
{
    [SerializeField] private PlayerCamera playerCamera;

    private Button quitButton;
    private Button resumeButton;
    private SliderInt sensSlider;
    private SliderInt maxFpsSlider;

    private int sensValue = 5;
    private int maxFpsValue = 120;

    protected override void OnOpen()
    {
        PauseManager.Pause();

        sensValue = SaveManager.Instance.Get(Opts.CameraSensitivity);
        maxFpsValue = SaveManager.Instance.Get(Opts.MaxFramerate);
    }

    protected override void OnClose()
    {
        PauseManager.Unpause();

        SaveManager.Instance.Set(Opts.CameraSensitivity, sensValue);
        SaveManager.Instance.Set(Opts.MaxFramerate, maxFpsValue);
        SaveManager.Instance.Save();
    }

    protected override void OnRootReady()
    {
        quitButton = root.Q<Button>("QUIT");
        quitButton.clicked += () => Application.Quit();

        resumeButton = root.Q<Button>("RESUME");
        resumeButton.clicked += Close;

        sensSlider = root.Q<SliderInt>("Sens");
        sensSlider.value = sensValue;
        sensSlider.RegisterValueChangedCallback(OnSensSliderValueChanged);

        maxFpsSlider = root.Q<SliderInt>("MaxFPS");
        maxFpsSlider.value = maxFpsValue;
        maxFpsSlider.RegisterValueChangedCallback(OnMaxFpsSliderValueChanged);
    }

    void OnSensSliderValueChanged(ChangeEvent<int> sens)
    {
        playerCamera.SetCameraSensitivity((float)sens.newValue/10);
        sensValue = sens.newValue;
    }

    void OnMaxFpsSliderValueChanged(ChangeEvent<int> fps)
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = fps.newValue;
        maxFpsValue = fps.newValue;
    }

    public void Toggle()
    {
        if (IsOpen)
            Close();
        else
            Open(null);
    }
}