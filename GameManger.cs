using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalObjectsToCatch = 5;
    private int objectsCaught = 0;

    public Light pointLight; // Assign this in the Inspector
    public float increaseAmount = 1f; // Amount to increase light each time
    public float maxIntensity = 10f; // Maximum light intensity
    public float minIntensity = 0.5f; // Minimum light intensity

    public AudioSource audioSource; // Assign this in the Inspector
    public AudioClip catchSound; // Assign your sound clip in the Inspector

    private void Start()
    {
        if (pointLight == null)
        {
            Debug.LogError("Point Light not set!");
        }

        if (audioSource == null || catchSound == null)
        {
            Debug.LogError("AudioSource or CatchSound not set!");
        }

        // Set initial light intensity to minimum
        pointLight.intensity = minIntensity;
    }

    public void ObjectCaught()
    {
        objectsCaught++;

        // Increase light intensity
        if (pointLight != null)
        {
            pointLight.intensity = Mathf.Min(maxIntensity, pointLight.intensity + increaseAmount);
        }

        // Play catch sound
        if (audioSource != null && catchSound != null)
        {
            audioSource.PlayOneShot(catchSound);
        }

        if (objectsCaught >= totalObjectsToCatch)
        {
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("Scenes_nature/Tree_scene");
    }
}
