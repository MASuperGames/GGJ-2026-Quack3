using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f; // Degrees per second

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
}