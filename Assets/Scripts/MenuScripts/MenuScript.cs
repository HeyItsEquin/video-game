using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public abstract class MenuScript : MonoBehaviour
{
    [SerializeField] private bool startOpen;

    protected UIDocument document;
    protected VisualElement root;
    protected MenuScript lastOpenedBy;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        IsOpen = startOpen;
        if (startOpen) OnOpen();
    }
    private void OnEnable()
    {
        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;

        root.style.display = IsOpen ? DisplayStyle.Flex : DisplayStyle.None;

        OnRootReady();
    }

    protected virtual void OnRootReady() { }
    protected virtual void OnOpen() { }
    protected virtual void OnClose() { }

    public void Open(MenuScript opener)
    {
        lastOpenedBy = opener;
        Show();
    }

    public void Close()
    {
        root.style.display = DisplayStyle.None;
        IsOpen = false;
        OnClose();
    }

    protected void Back()
    {
        var target = lastOpenedBy;
        Close();
        target?.Show(); // "Unity objects should not use null propagation" shut up
    }

    private void Show()
    {
        root.style.display = DisplayStyle.Flex;
        IsOpen = true;
        OnOpen();
    }
}