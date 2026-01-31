using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    enum State
    {
        Slow,
        Attack,
        Backoff
    };

    [SerializeField] private float hitDistance = 0.5f;
    [SerializeField] private float damage = 10;
    [SerializeField] private float damageFrequency = 1;

    [SerializeField] private float hitSphereDistance = 1;
    [SerializeField] private float hitSphereRadius = 1;



    [SerializeField] private float attackDistance = 8;
    [SerializeField] private float backoffDistance = 4;
    [SerializeField] private float waitTime = 2;

    [SerializeField] private float attackSpeed = 3.5f;
    [SerializeField] private float slowSpeed = 1;

    [SerializeField] private GameObject quad;

    [SerializeField] private float flipDistance = 1;

    private float accDistance;
    private Vector3 prevPosition;

    private NavMeshAgent navMeshAgent;
    private CapsuleCollider capsule;

    State state = State.Slow;
    float backoffStart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        capsule = GetComponent<CapsuleCollider>();
        accDistance = 0;
        prevPosition = transform.position;
    }

    public void onHealthChange(float health, float delta)
    {
        
    }

    public void onHealthDepleted()
    {
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
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

        accDistance += Vector3.Distance(transform.position, prevPosition);
        prevPosition = transform.position;
        if (accDistance > flipDistance)
        {
            accDistance -= flipDistance;
            Vector3 newScale = quad.transform.localScale;
            newScale.x *= -1;
            quad.transform.localScale = newScale;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(
            transform.position + hitSphereDistance * transform.forward,
            hitSphereRadius
        );
    }
}
