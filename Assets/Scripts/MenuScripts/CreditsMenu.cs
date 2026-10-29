using UnityEngine.UIElements;

public class CreditsMenu : MenuScript
{
    private Button backButton;

    protected override void OnRootReady()
    {
        backButton = root.Q<Button>("BackButton");
        backButton.clicked += OnBackClicked;

        
    }

    void OnBackClicked()
    {
        Back();
    }
}