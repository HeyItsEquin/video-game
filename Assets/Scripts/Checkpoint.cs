using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool beenActivated = false;
    private ParticleSystem activationParticles;

    private void Start()
    {
        GetComponent<ParticleSystem>().Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter character = other.transform.parent.GetComponent<PlayerCharacter>();
        if (character != null && !beenActivated)
        {
            activationParticles = GetComponentInChildren<ParticleSystem>();
            activationParticles.Play();
            beenActivated = true;
            character.lastCheckpoint = this;
        }
    }

}
