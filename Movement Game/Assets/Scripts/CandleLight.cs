using UnityEngine;

public class CandleLight : MonoBehaviour
{
    private Light candleLight;
    private float baseIntensity;
    public float flickerAmount = 0.3f;
    public float flickerSpeed = 0.1f; // in seconds

    private float flickerTimer = 0f;

    void Start()
    {
        candleLight = GetComponent<Light>();
        baseIntensity = candleLight.intensity;
    }

    void Update()
    {
        flickerTimer += Time.deltaTime;

        if (flickerTimer >= flickerSpeed)
        {
            candleLight.intensity = baseIntensity + Random.Range(-flickerAmount, flickerAmount);
            flickerTimer = 0f;
        }
    }
}

