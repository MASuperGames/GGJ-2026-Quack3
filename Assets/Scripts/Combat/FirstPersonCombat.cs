using Unity.VisualScripting;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.VFX;

public class FirstPersonCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Weapon primaryWeapon;
    [SerializeField] private Weapon secondaryWeapon;
    [SerializeField] private VisualEffect gunHitVFX;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Header("Audio")]
    [SerializeField] private AudioClip[] primaryAttackClips;
    [SerializeField] private AudioClip[] secondaryAttackClips;
    [SerializeField] private AudioClip[] hitImpactClips;

    [Header("Combat Settings")]
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private float damage = 10;

    public bool boneFragmentMode = false;
    public bool zombieFleshmode = false;
    
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
            primaryWeapon.GetAnimator().SetTrigger("Attack");
            primaryWeapon.PlayVFX();
            impulseSource.GenerateImpulse();

            PlayRandomClip(primaryAttackClips);

            var res = Physics.OverlapSphere(transform.position + attackDistance * transform.forward, attackRadius);
            bool hitEnemy = false;
            foreach (var col in res)
            {
                if (col.gameObject == gameObject) continue;
                var health = col.GetComponent<HealthManager>();
                if (health == null) continue;
                health.ChangeHealth(-damage * (zombieFleshmode ? 2 : 1));
                hitEnemy = true;
            }

            if (hitEnemy)
            {
                PlayRandomClip(hitImpactClips);
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
            secondaryWeapon.GetAnimator().SetTrigger("Attack");
            secondaryWeapon.PlayVFX();
            impulseSource.GenerateImpulse();

            PlayRandomClip(secondaryAttackClips);

            var cam = Camera.main;
            var ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            Debug.DrawLine(cam.transform.position, 10000 * cam.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                gunHitVFX.gameObject.transform.position = hit.point;
                gunHitVFX.SendEvent("OnPlay");
                var health = hit.collider.GetComponent<HealthManager>();
                if (health != null)
                {
                    health.ChangeHealth(-damage * (boneFragmentMode ? 2 : 1));
                    //PlayRandomClip(hitImpactClips);
                }
                   
            }
        }
        else
        {
            // Release
        }
    }

    private void PlayRandomClip(AudioClip[] clips)
    {
        if (clips != null && clips.Length > 0)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            AudioManager.Instance.PlaySFXWithPitchVariation(clip);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + attackDistance * transform.forward, attackRadius);
    }
}
