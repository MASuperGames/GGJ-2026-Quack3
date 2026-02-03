using Unity.VisualScripting;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.VFX;
using UnityEngine.Events;

public class FirstPersonCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Weapon primaryWeapon;
    [SerializeField] private Weapon secondaryWeapon;
    [SerializeField] private Weapon tertiaryWeapon;
    [SerializeField] private VisualEffect gunHitVFX;
    [SerializeField] private ParticleSystem gunHitPS;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Header("Audio")]
    [SerializeField] private AudioClip[] primaryAttackClips;
    [SerializeField] private AudioClip[] secondaryAttackClips;
    [SerializeField] private AudioClip[] hitImpactClips;
    [SerializeField] private AudioClip[] tauntClips;

    [Header("Combat Settings")]
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private float damage = 10;
    
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private int ammo = 30;

    public UnityEvent<int> onAmmoChange;

    public void AddAmmo(int amount)
    {
        ammo += amount;
        if (ammo > maxAmmo) ammo = maxAmmo;
        onAmmoChange?.Invoke(ammo);
    }

    public int getCurrentAmmo()
    {
        return ammo;
    }

    public bool boneFragmentMode = false;
    public bool zombieFleshmode = false;
    
    private void OnEnable()
    {
        inputReader.PrimaryActionEvent += OnPrimaryAction;
        inputReader.SecondaryActionEvent += OnSecondaryAction;
        inputReader.InteractEvent += OnTertiaryAction;
    }

    private void OnDisable()
    {
        inputReader.PrimaryActionEvent -= OnPrimaryAction;
        inputReader.SecondaryActionEvent -= OnSecondaryAction;
        inputReader.InteractEvent -= OnTertiaryAction;
    }

    void Start()
    {
        
    }

    void Update()
    {
    
    }

    private void OnPrimaryAction(bool isPressed)
    {
        if (PauseManager.Instance.IsPaused()) return;
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
        if (PauseManager.Instance.IsPaused()) return;
        if (isPressed)
        {
            if (ammo == 0)
            {
                return;
            }
            --ammo;
            onAmmoChange?.Invoke(ammo);
            secondaryWeapon.GetAnimator().SetTrigger("Attack");
            secondaryWeapon.PlayVFX();
            impulseSource.GenerateImpulse();

            PlayRandomClip(secondaryAttackClips);

            var cam = Camera.main;
            var ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            Debug.DrawLine(cam.transform.position, 10000 * cam.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (gunHitVFX)
                {
                    gunHitVFX.gameObject.transform.position = hit.point;
                    gunHitVFX.SendEvent("OnPlay");
                }
                if (gunHitPS)
                {
                    gunHitPS.gameObject.transform.position = hit.point;
                    gunHitPS.gameObject.transform.rotation = Quaternion.LookRotation(hit.normal);
                    gunHitPS.Play();
                }

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

    private void OnTertiaryAction()
    {
        if (tauntClips != null && tauntClips.Length > 0)
        {
            AudioClip clip = tauntClips[Random.Range(0, tauntClips.Length)];
            AudioManager.Instance.PlaySFXWithPitchVariation(clip, 0.75f, 0.9f);
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
