using UnityEngine.UIElements;

public class OptionsMenu : MenuScript
{
    private Button gameplayOptions;
    private Button videoOptions;
    private Button controlOptions;
    private Button audioOptions;
    private Button miscOptions;

    private Button navButtonLeft;
    private Button navButtonRight;

    private Button applyButton;
    private Button resetButton;
    private Button cancelButton;

    protected override void OnRootReady()
    {
        gameplayOptions = root.Q<Button>("GameplayOptions");
        gameplayOptions.clicked += OnGameplayOptions;

        videoOptions = root.Q<Button>("VideoOptions");
        videoOptions.clicked += OnVideoOptions;

        controlOptions = root.Q<Button>("ControlOptions");
        controlOptions.clicked += OnControlOptions;

        audioOptions = root.Q<Button>("AudioOptions");
        audioOptions.clicked += OnAudioOptions;

        miscOptions = root.Q<Button>("MiscOptions");
        miscOptions.clicked += OnMiscOptions;

        navButtonLeft = root.Q<Button>("NavButtonLeft");
        navButtonRight = root.Q<Button>("NavButtonRight");

        applyButton = root.Q<Button>("ApplyButton");
        resetButton = root.Q<Button>("ResetButton");
        cancelButton = root.Q<Button>("CancelButton");

        cancelButton.clicked += OnCancel;
    }

    void OnGameplayOptions()
    {
        
    }
    void OnVideoOptions()
    {
        
    }
    void OnControlOptions()
    {
        
    }
    void OnAudioOptions()
    {
        
    }
    void OnMiscOptions()
    {
        
    }
    
    void OnNavButtonLeft()
    {
        
    }
    void OnNavButtonRight()
    {
        
    }
    
    void OnCancel()
    {
        Back();
    }
}