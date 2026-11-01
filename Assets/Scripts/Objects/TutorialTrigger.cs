using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    TutorialMenu tutorialMenu;

    private bool activated = false;

    void Start()
    {
        tutorialMenu = GetComponentInChildren<TutorialMenu>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!activated)
        {
            tutorialMenu.Open(null);
            activated = true;            
        }
    }
}