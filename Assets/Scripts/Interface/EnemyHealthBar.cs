using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image sliderImage;
    private float maxHealth;


    private void Start()
    {
        maxHealth = GetComponent<HealthManager>().MaxHealth;
    }

    public void HandleHealthChanged(float health, float delta)
    {
        sliderImage.fillAmount = health / maxHealth;
    }
}
