using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StartLoopAtRandomPos : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Check if an AudioClip is assigned to prevent errors
        if (audioSource.clip == null)
        {
            Debug.LogError("AudioSource on " + gameObject.name + " requires an AudioClip assigned to its 'Clip' slot.");
            return;
        }

        // Determine the length of the clip
        float clipLength = audioSource.clip.length;

        // Generate a random time within the clip's duration
        float randomStartTime = Random.Range(0f, clipLength);

        // Set the AudioSource's time to the random start point
        audioSource.time = randomStartTime;

        // Start playing the audio from the random position
        audioSource.Play();
    }
}
