using UnityEngine;
using UnityEngine.VFX;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private VisualEffect vfx;

    public void PlayVFX()
    {
        if (vfx != null)
        {
            vfx.SendEvent("OnPlay");
        }

    }

    public Animator GetAnimator()
    {
        return animator;
    }
}
