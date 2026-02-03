using TMPro;
using UnityEngine;

public class DeathCounter : Singleton<DeathCounter>
{
    private int deathCount = 0;
    [SerializeField] private TextMeshProUGUI deathCounterText;

    public void UpdateUI()
    {
        deathCounterText.text = "deaths: " + deathCount;
    }

    public void CountDeath()
    {
        deathCount++;
        UpdateUI();
    }
}
