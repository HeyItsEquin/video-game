using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool beenActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter character = other.transform.parent.GetComponent<PlayerCharacter>();
        if (character != null && !beenActivated)
        {
            beenActivated = true;
            character.lastCheckpoint = this;
        }
    }

}
