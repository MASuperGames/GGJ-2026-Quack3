using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BossEnemy : MonoBehaviour
{
    enum State
    {
        Idle,
        Active,
        Dead
    };

    State state = State.Idle;

    float deathStart;

    int stage = 1;

    public UnityEvent<int> stageChange;

    private HealthManager healthManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
    }

    public void onHealthChange(float health, float delta)
    {
        if (stage == 1 && (health / healthManager.MaxHealth) < 0.666f)
        {
            stage = 2;
            stageChange.Invoke(stage);
        }

        if (stage == 2 && (health / healthManager.MaxHealth) < 0.333f)
        {
            stage = 3;
            stageChange.Invoke(stage);
        }
    }

    public void onHealthDepleted()
    {
        state = State.Dead;
        deathStart = Time.time;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        SceneManager.LoadScene(4);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Dead)
        {
            //animator.enabled = true;

            //if (deathStart + deathDuration < Time.time)
            {
                Destroy(gameObject);
            }
            return;
        }

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector3 target = player.transform.position - transform.position;
        target.y = 0;
        target = Vector3.Normalize(target);
        transform.forward = Vector3.Normalize(transform.forward + 0.3f * Time.deltaTime * target);
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(
        //    transform.TransformPoint(capsule.center) + hitSphereDistance * transform.forward,
        //    hitSphereRadius
        //);
    }
}
