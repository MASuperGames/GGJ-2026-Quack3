using UnityEngine;

public class VolumeController : MonoBehaviour
{
    public void OnVolumeSliderValueChanged(float value)
    {
        AudioManager.Instance.SetMasterVolume(value);
    }
}
