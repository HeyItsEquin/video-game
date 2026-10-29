using System;
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

    private OptionCategory currentTab = OptionCategory.Gameplay;

    protected override void OnRootReady()
    {
        gameplayOptions = root.Q<Button>("GameplayOptions");
        gameplayOptions.clicked += () => SwitchTab(OptionCategory.Gameplay);

        videoOptions = root.Q<Button>("VideoOptions");
        videoOptions.clicked += () => SwitchTab(OptionCategory.Video);

        controlOptions = root.Q<Button>("ControlOptions");
        controlOptions.clicked += () => SwitchTab(OptionCategory.Controls);

        audioOptions = root.Q<Button>("AudioOptions");
        audioOptions.clicked += () => SwitchTab(OptionCategory.Audio);

        miscOptions = root.Q<Button>("MiscOptions");
        miscOptions.clicked += () => SwitchTab(OptionCategory.Misc);

        navButtonLeft = root.Q<Button>("NavButtonLeft");
        navButtonLeft.clicked += OnNavButtonLeft;
        
        navButtonRight = root.Q<Button>("NavButtonRight");
        navButtonRight.clicked += OnNavButtonRight;

        applyButton = root.Q<Button>("ApplyButton");

        resetButton = root.Q<Button>("ResetButton");

        cancelButton = root.Q<Button>("CancelButton");
        cancelButton.clicked += OnCancel;

        SwitchTab(OptionCategory.Gameplay);
    }

    OptionCategory GetPreviousCategory()
    {
        var newIdx = (int)currentTab-1;
        if (Enum.IsDefined(typeof(OptionCategory), newIdx))
        {
            return (OptionCategory)newIdx;
        }
        return currentTab;
    }

    OptionCategory GetNextCategory()
    {
        var newIdx = (int)currentTab+1;
        if (Enum.IsDefined(typeof(OptionCategory), newIdx))
        {
            return (OptionCategory)newIdx;
        }
        return currentTab;
    }

    Button CurrentTabButton()
    {
        switch(currentTab)
        {
            case OptionCategory.Gameplay:
                return root.Q<Button>("GameplayOptions");
            case OptionCategory.Video:
                return root.Q<Button>("VideoOptions");
            case OptionCategory.Controls:
                return root.Q<Button>("ControlOptions");
            case OptionCategory.Audio:
                return root.Q<Button>("AudioOptions");
            case OptionCategory.Misc:
                return root.Q<Button>("MiscOptions");
            default:
                return root.Q<Button>("GameplayOptions");
        }
    }

    void SwitchTab(OptionCategory newTab)
    {
        var currentButton = CurrentTabButton();
        currentTab = newTab;
        var newButton = CurrentTabButton();

        currentButton.RemoveFromClassList("active");
        newButton.AddToClassList("active");

        LoadTabOptions();
    }
    
    void LoadTabOptions()
    {
        VisualElement container = root.Q<VisualElement>("OptionNames");
        container.Clear();
        foreach (var e in OptionRegistry.Get(currentTab))
            container.Add(e.CreateControl());
    }

    void OnNavButtonLeft()
    {
        SwitchTab(GetPreviousCategory());
    }
    void OnNavButtonRight()
    {
        SwitchTab(GetNextCategory());
    }
    
    void OnCancel()
    {
        Back();
    }
}