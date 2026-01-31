using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class ShootEnemy : MonoBehaviour
{
    enum State
    {
        Live,
        Dead
    };

    [SerializeField] private GameObject boneFragment;

    [SerializeField] private float deviationStrength = 1;
    [SerializeField] private float targetStrength = 1;

    [SerializeField] private float rotationStrength = 10;

    [SerializeField] private GameObject projectile;
    [SerializeField] private float shootInterval;

    [SerializeField] private float deathDuration;

    private Vector3 trajectory;
    private Vector3 deviationTrajectory;

    private Vector3 lookDirection;



    private State state = State.Live;

    private float deathStart;
    private float lastShot;

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
        lastShot = -Mathf.Infinity;
    }

    public void onHealthChange(float health, float delta)
    {
        
    }

    public void onHealthDepleted()
    {
        state = State.Dead;
        deathStart = Time.time;

        Instantiate(boneFragment, transform.position, Quaternion.identity);
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
        
        if (lastShot + shootInterval < Time.time)
        {
            Vector3 toPlayer = Vector3.Normalize(player.transform.position - transform.position);
            Instantiate(projectile, transform.position + 2.0f * toPlayer, Quaternion.LookRotation(toPlayer));
            lastShot = Time.time;
        }
    }

    void OnDrawGizmos()
    {
    }
}
