using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public UnityEvent<float> onHealthChange;
    public UnityEvent onHealthDepleted;

    public float MaxHealth = 100;
    public float Health = 100;

    void ChangeHealth(float delta)
    {
        float PrevHealth = Health;
        Health = Mathf.Clamp(Health + delta, 0, MaxHealth);
        if (Health == PrevHealth) return;
        onHealthChange.Invoke(Health);
        if (Health == 0)
            onHealthDepleted.Invoke();
    }
}
