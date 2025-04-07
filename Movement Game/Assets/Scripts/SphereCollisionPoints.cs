using UnityEngine;
using UnityEngine.UI;

public class SphereCollisionPoints : MonoBehaviour
{
    public Slider orbSlider; // Assign this in the Inspector

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("orb"))
        {
            // Deactivate the orb
            collision.gameObject.SetActive(false);

            // Increase slider value by 1, clamped to max value
            if (orbSlider != null)
            {
                orbSlider.value = Mathf.Min(orbSlider.value + 1, orbSlider.maxValue);
            }
        }
    }
}