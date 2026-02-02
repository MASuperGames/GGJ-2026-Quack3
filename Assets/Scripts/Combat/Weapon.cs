using UnityEngine;
using UnityEngine.VFX;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private VisualEffect vfx;
    [SerializeField] private ParticleSystem[] ps;

    public void PlayVFX()
    {
        if (vfx != null)
        {
            vfx.SendEvent("OnPlay");
        }

        if (ps != null && ps.Length > 0)
        {
            foreach (ParticleSystem ps in ps)
            {
                if (ps != null)
                {
                    ps.Play();
                }
            }
        }

    }

    public Animator GetAnimator()
    {
        return animator;
    }
}
