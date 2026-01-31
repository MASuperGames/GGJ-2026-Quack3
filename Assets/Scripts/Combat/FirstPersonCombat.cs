using Unity.VisualScripting;
using UnityEngine;
using Unity.Cinemachine;

public class FirstPersonCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Animator primaryAnimator;
    [SerializeField] private Animator secondaryAnimator;
    [SerializeField] private CinemachineImpulseSource impulseSource;


    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private float damage = 10;

    private void OnEnable()
    {
        inputReader.PrimaryActionEvent += OnPrimaryAction;
        inputReader.SecondaryActionEvent += OnSecondaryAction;
    }

    private void OnDisable()
    {
        inputReader.PrimaryActionEvent -= OnPrimaryAction;
        inputReader.SecondaryActionEvent -= OnSecondaryAction;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnPrimaryAction(bool isPressed)
    {
        if (isPressed)
        {
            primaryAnimator.SetTrigger("Attack");
            impulseSource.GenerateImpulse();

            var res = Physics.OverlapSphere(transform.position + attackDistance * transform.forward, attackRadius);
            foreach (var col in res)
            {
                if (col.gameObject == gameObject) continue;
                var health = col.GetComponent<HealthManager>();
                if (health == null) continue;
                health.ChangeHealth(-damage);
            }
        }
        else
        {
            // Release
        }

    }

    private void OnSecondaryAction(bool isPressed)
    {
        if (isPressed)
        {
            secondaryAnimator.SetTrigger("Attack");
            impulseSource.GenerateImpulse();

            var res = Physics.OverlapSphere(transform.position + attackDistance * transform.forward, attackRadius);
            foreach (var col in res)
            {
                if (col.gameObject == gameObject) continue;
                var health = col.GetComponent<HealthManager>();
                if (health == null) continue;
                health.ChangeHealth(-damage);
            }
        }
        else
        {
            // Release
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + attackDistance * transform.forward, attackRadius);
    }
}
