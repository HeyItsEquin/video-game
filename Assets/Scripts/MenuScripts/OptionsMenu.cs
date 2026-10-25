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
        videoOptions = root.Q<Button>("VideoOptions");
        controlOptions = root.Q<Button>("ControlOptions");
        audioOptions = root.Q<Button>("AudioOptions");
        miscOptions = root.Q<Button>("MiscOptions");

        navButtonLeft = root.Q<Button>("NavButtonLeft");
        navButtonRight = root.Q<Button>("NavButtonRight");

        applyButton = root.Q<Button>("ApplyButton");
        resetButton = root.Q<Button>("ResetButton");
        cancelButton = root.Q<Button>("CancelButton");


    }
}