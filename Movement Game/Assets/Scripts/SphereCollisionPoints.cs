using UnityEngine;
using UnityEngine.UI;

public class SphereCollisionPoints : MonoBehaviour
{
    public Slider orbSlider;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("orb"))
        {
            Debug.Log("Collided with orb!");

            PointsData.playerScore += 1;
            Debug.Log(PointsData.playerScore);

            // Find ambient and ping sounds
            AudioSource[] audioSources = collision.gameObject.GetComponents<AudioSource>();
            AudioSource ambientAudio = null;
            AudioSource pingAudio = null;

            foreach (AudioSource src in audioSources)
            {
                if (src.loop)
                    ambientAudio = src;
                else
                    pingAudio = src;
            }

            // Stop ambient audio
            if (ambientAudio != null)
                ambientAudio.Stop();

            // Play ping sound
            if (pingAudio != null)
            {
                pingAudio.Play();
            }

            // Disable collider
            Collider orbCollider = collision.gameObject.GetComponent<Collider>();
            if (orbCollider != null)
                orbCollider.enabled = false;

            // Hide all renderers
            foreach (Renderer r in collision.gameObject.GetComponentsInChildren<Renderer>())
            {
                r.enabled = false;
            }

            // Disable any point lights under the orb
            foreach (Light light in collision.gameObject.GetComponentsInChildren<Light>())
            {
                light.enabled = false;
            }

            // Increase slider
            if (orbSlider != null)
            {
                orbSlider.value = Mathf.Min(orbSlider.value + 1, orbSlider.maxValue);
                Debug.Log("Slider value increased to: " + orbSlider.value);
            }

            // Destroy orb after ping finishes
            float destroyDelay = (pingAudio != null && pingAudio.clip != null) ? pingAudio.clip.length : 0f;
            Destroy(collision.gameObject, destroyDelay);
        }
    }
}
