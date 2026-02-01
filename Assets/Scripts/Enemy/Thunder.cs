using UnityEngine;

public class Thunder : MonoBehaviour
{
    [SerializeField] private float dps = 30;
    [SerializeField] private float audioFreq = 10f;

    [SerializeField] private bool damageEnabled = false;

    [SerializeField] private AudioClip[] thunderClips;

    [SerializeField] private bool playAudio = false;

    private float startedTime;
    private float nextAudioTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextAudioTime = -Mathf.Infinity;
        startedTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (damageEnabled)
        {
            var res = Physics.OverlapCapsule(transform.position, transform.position + new Vector3(0, 10, 0), 5);
            foreach (var col in res)
            {
                if (col.tag != "Player") continue;
                col.GetComponent<HealthManager>()?.ChangeHealth(-dps * Time.deltaTime);
            }
        }

        if (playAudio && nextAudioTime < Time.time)
        {
            AudioClip clip = thunderClips[Random.Range(0, thunderClips.Length)];
            AudioManager.Instance.PlaySFX(clip, 0.1f);
            nextAudioTime = Time.time + 1.0f / audioFreq; 
        }

        if (startedTime + 8.0f < Time.time)
            Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 5);
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 10, 0), 5);
    }
}
