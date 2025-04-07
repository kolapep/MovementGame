using UnityEngine;
using TMPro;

public class UIChanges : MonoBehaviour
{
    public TextMeshProUGUI pointsText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int collectedPoints = PointsData.playerScore;
        pointsText.text = "Energy Collected:" + (collectedPoints);

    }

}
