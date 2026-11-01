using UnityEngine;
using UnityEngine.UIElements;

public class TutorialMenu : MenuScript
{
    Button okButton;
    
    [SerializeField] string tutorialTitle;
    [SerializeField, TextArea(3, 10)] string content;

    Label title;
    Label tutorialText;

    protected override void OnOpen()
    {
        PauseManager.Pause();
    }

    protected override void OnClose()
    {
        PauseManager.Unpause();
    }

    protected override void OnRootReady()
    {
        okButton = root.Q<Button>("OkButton");
        Debug.Log(okButton);
        okButton.clicked += Close;

        title = root.Q<Label>("Title");
        title.text = tutorialTitle;
        tutorialText = root.Q<Label>("TutorialText"); // Supports RT
        tutorialText.text = content;
    }
}