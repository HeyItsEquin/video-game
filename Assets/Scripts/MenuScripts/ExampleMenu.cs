using UnityEngine;
using UnityEngine.UIElements;

public class ExampleMenu : MenuScript
{
    [SerializeField] private MenuScript otherMenu;

    private Button exampleButton;
    private Button openOtherMenu;
    private Button backButton;

    // Called when this menu is opened
    // UI Logic should not go inside OnOpen because root is not guaranteed to exist yet
    protected override void OnOpen()
    {
        Debug.Log("Example menu was just opened");
    }
    
    // Called when this menu is closed
    protected override void OnClose()
    {
        Debug.Log("Example menu was just closed");
    }

    // Called when both the document and root elements are available
    protected override void OnRootReady()
    {
        exampleButton = root.Q<Button>("ExampleButton");
        exampleButton.clicked += OnExampleClicked;

        openOtherMenu = root.Q<Button>("OpenOtherMenu");
        openOtherMenu.clicked += OnOpenOtherMenuClicked;

        backButton = root.Q<Button>("BackButton");
        backButton.clicked += OnBackClicked;
    }

    void OnExampleClicked()
    {
        Debug.Log("Pressed example button");
    }

    void OnOpenOtherMenuClicked()
    {
        // Whenever calling MenuScript.Open(), always pass the current menu in via this
        otherMenu.Open(this);
        Close();
    }

    void OnBackClicked()
    {
        // Back() closes the current menu and opens the menu which last opened it
        Back();
    }
}