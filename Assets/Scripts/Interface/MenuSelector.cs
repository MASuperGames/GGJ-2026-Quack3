using UnityEngine;
using UnityEngine.UI;

public class MenuSelector : MonoBehaviour
{
    [SerializeField] private Button firstButton;
    [SerializeField] private AudioClip selectClip;

    void Start()
    {
        firstButton.Select();
    }

    public void PlaySelectSound()
    {
        AudioManager.Instance.PlaySFX(selectClip, 0.2f);
    }
}