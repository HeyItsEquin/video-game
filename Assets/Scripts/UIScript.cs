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
    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        startButton = root.Q<Button>("START");
        quitButton = root.Q<Button>("QUIT");

        startButton.clicked += () => SceneManager.LoadScene(startButtonScene);
        quitButton.clicked += () => Application.Quit();
    }


}
