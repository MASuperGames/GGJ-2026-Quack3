using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class ShootEnemy : MonoBehaviour
{
    enum State
    {
        Attack,
        Fly,
        Dead
    };

    [SerializeField] private float deviationStrength = 1;
    [SerializeField] private float targetStrength = 1;

    [SerializeField] private float rotationStrength = 10;



    private Vector3 trajectory;
    private Vector3 deviationTrajectory;

    private Vector3 lookDirection;


    [SerializeField] private float trajectorySpread;

    [SerializeField] private float deathDuration;

    State state = State.Fly;

    float deathStart;

    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        trajectory = new Vector3(1, 0, 0);
        lookDirection = new Vector3(1, 0, 0);
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            trajectory = Vector3.Normalize(player.transform.position - transform.position);
            lookDirection = Vector3.Normalize(player.transform.position - transform.position);
        }
        deviationTrajectory = UnityEngine.Random.onUnitSphere;
    }

    public void onHealthChange(float health, float delta)
    {
        
    }

    public void onHealthDepleted()
    {
        state = State.Dead;
        deathStart = Time.time;
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

        {
            Vector3 targetPosition = player.transform.position + new Vector3(0, 2, 0);
            Vector3 targetDirection = Vector3.Normalize(targetPosition - transform.position);

            deviationTrajectory = Vector3.Normalize(deviationTrajectory + deviationStrength * Time.deltaTime * UnityEngine.Random.onUnitSphere);
            trajectory = Vector3.Normalize(trajectory + deviationStrength * Time.deltaTime * deviationTrajectory);
            trajectory = Vector3.Normalize(trajectory + targetStrength * Time.deltaTime * targetDirection);
        }

        {
            Vector3 targetDirection = player.transform.position - transform.position;
            lookDirection = Vector3.Normalize(lookDirection + rotationStrength * Time.deltaTime * targetDirection);
            transform.forward = lookDirection;
        }

        transform.position += trajectory * Time.deltaTime;
    }

    void OnDrawGizmos()
    {
    }
}
