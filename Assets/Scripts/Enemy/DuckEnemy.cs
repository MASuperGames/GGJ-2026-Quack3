using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Timeline;

public class DuckEnemy : MonoBehaviour
{
    enum State
    {
        Slow,
        Attack,
        Backoff,
        Dead
    };

    [Header("Audio")]
    [SerializeField] private AudioSource sourceFoot;
    [SerializeField] private AudioSource sourceQuack;
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private float pitchVariationMin = 0.95f;
    [SerializeField] private float pitchVariationMax = 1.05f;


    [SerializeField] private AudioClip[] quackClips;
    [SerializeField] private AudioClip[] angryQuackClips;
    [SerializeField] private float quackFrequency = 1.0f;
    [SerializeField] private float angryQuackFrequency = 3.0f;



    [SerializeField] private float damage = 10;
    [SerializeField] private float hitSphereDistance = 1;
    [SerializeField] private float hitSphereRadius = 1;

    [SerializeField] private float attackDistance = 8;
    [SerializeField] private float backoffDistance = 4;
    [SerializeField] private float waitTime = 2;

    [SerializeField] private float attackSpeed = 3.5f;
    [SerializeField] private float slowSpeed = 1;

    [SerializeField] private GameObject quad;

    [SerializeField] private float flipDistance = 1;

    [SerializeField] private float deathDuration = 2;

    [SerializeField] private GameObject feather;

    private AudioClip selectedQuackSound;
    private AudioClip selectedAngryQuackSound;
    private float lastQuackSound = -Mathf.Infinity;

    private float accDistance;
    private Vector3 prevPosition;

    private NavMeshAgent navMeshAgent;
    [SerializeField] private CapsuleCollider capsule;

    State state = State.Slow;
    float backoffStart;

    float deathStart;

    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        capsule = GetComponent<CapsuleCollider>();
        accDistance = 0;
        prevPosition = transform.position;
        animator = GetComponent<Animator>();

        selectedQuackSound = quackClips[UnityEngine.Random.Range(0, quackClips.Length)];
        selectedAngryQuackSound = angryQuackClips[UnityEngine.Random.Range(0, angryQuackClips.Length)];
    }

    public void onHealthChange(float health, float delta)
    {
        
    }

    public void onHealthDepleted()
    {
        state = State.Dead;
        deathStart = Time.time;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        if (UnityEngine.Random.value < 0.5f)
        {
            Instantiate(feather, transform.TransformPoint(feather.transform.position), transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Dead)
        {
            animator.enabled = true;

            if (deathStart + deathDuration < Time.time)
            {
                Destroy(gameObject);
            }
            return;
        }

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        bool hasAttackDistance = Vector3.Distance(transform.position, player.transform.position) < attackDistance;
        if (state == State.Slow && hasAttackDistance)
            state = State.Attack;
        if (state != State.Slow && !hasAttackDistance)
            state = State.Slow;
        if (state == State.Backoff && backoffStart + waitTime < Time.time)
            state = State.Attack;
        if (state == State.Attack) {
            var res = Physics.OverlapSphere(
                transform.position + hitSphereDistance * transform.forward,
                hitSphereRadius
            );
            foreach (var collider in res) {
                if (collider.gameObject.tag != "Player")
                    continue;
                collider.GetComponent<HealthManager>()?.ChangeHealth(-damage);
                state = State.Backoff;
                backoffStart = Time.time;
                Vector3 backoffDirection = Vector3.Normalize(player.transform.position - transform.position);
                navMeshAgent.destination = player.transform.position - backoffDistance * backoffDirection;
                navMeshAgent.speed = attackSpeed;
            }
        }

        navMeshAgent.angularSpeed = state == State.Backoff ? 0 : 120;
        navMeshAgent.speed = state == State.Slow ? slowSpeed : attackSpeed;

        if (state == State.Slow || state == State.Attack)
            navMeshAgent.destination = player.transform.position;

        if (state == State.Slow || state == State.Attack)
        {
            AudioClip clip;
            float freq;
            if (state == State.Slow)
            {
                clip = selectedQuackSound;
                freq = quackFrequency;
            } else
            {
                clip = selectedAngryQuackSound;
                freq = angryQuackFrequency;
            }

            if (lastQuackSound + 1.0f / freq < Time.time)
            {
                sourceQuack.clip = clip;

                sourceQuack.volume = AudioManager.Instance.sfxVolume * AudioManager.Instance.masterVolume;
                sourceQuack.pitch = UnityEngine.Random.Range(1.0f, 1.0f);
                sourceQuack.Play();
                lastQuackSound = Time.time;
            }
        }

        accDistance += Vector3.Distance(transform.position, prevPosition);
        prevPosition = transform.position;
        if (accDistance > flipDistance)
        {
            PlayRandomClip(footsteps, 1.0f);
            accDistance -= flipDistance;
            Vector3 newScale = quad.transform.localScale;
            newScale.x *= -1;
            quad.transform.localScale = newScale;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(
            transform.TransformPoint(capsule.center) + hitSphereDistance * transform.forward,
            hitSphereRadius
        );
    }

    private void PlayRandomClip(AudioClip[] clips, float volumeScale = 1f)
    {
        if (clips != null && clips.Length > 0)
        {
            AudioClip clip = clips[UnityEngine.Random.Range(0, clips.Length)];
            //AudioManager.Instance.PlaySFXWithPitchVariation(clip, pitchVariationMin, pitchVariationMax, volumeScale);

            sourceFoot.clip = clip;

            sourceFoot.volume = AudioManager.Instance.sfxVolume * AudioManager.Instance.masterVolume;
            sourceFoot.pitch = UnityEngine.Random.Range(pitchVariationMin, pitchVariationMax);
            sourceFoot.Play();

        }
    }
}
