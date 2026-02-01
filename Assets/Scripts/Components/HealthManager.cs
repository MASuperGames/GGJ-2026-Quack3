using UnityEngine;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public UnityEvent<float, float> onHealthChange;
    public UnityEvent onHealthDepleted;

    [Header("Health Settings")]
    public float MaxHealth = 100;
    public float Health = 100;

    [Header("Audio")]
    [SerializeField] private AudioClip[] damageTakenClips;
    [SerializeField] private AudioClip[] deathClips;

    public void ChangeHealth(float delta)
    {

        float PrevHealth = Health;
        Health = Mathf.Clamp(Health + delta, 0, MaxHealth);
        Debug.Log("changing from " + PrevHealth + " to " + Health);
        if (Health == PrevHealth) return;

        if (delta < 0)
        {
            PlayRandomClip(damageTakenClips);
        }

        if (Health == 0)
        {
            PlayRandomClip(deathClips);
            onHealthDepleted.Invoke();
        }

        onHealthChange.Invoke(Health, Health - PrevHealth);




    }

    private void PlayRandomClip(AudioClip[] clips)
    {
        if (clips != null && clips.Length > 0)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            AudioManager.Instance.PlaySFX(clip);
        }
    }
}
