using UnityEngine;

public class PlayerMovementAudio : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    [Header("Footsteps")]
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float walkStepInterval = 0.5f;
    [SerializeField] private float sprintStepInterval = 0.3f;

    [Header("Jump & Land")]
    [SerializeField] private AudioClip[] jumpClips;
    [SerializeField] private AudioClip[] landClips;

    [SerializeField] private float pitchVariationMin = 0.95f;
    [SerializeField] private float pitchVariationMax = 1.05f;

    private float stepTimer;
    private bool wasGrounded = true;

    private void Update()
    {
        HandleFootsteps();
        HandleLanding();
    }

    private void HandleFootsteps()
    {
        bool isMoving = characterController.velocity.sqrMagnitude > 0.1f;
        bool isGrounded = characterController.isGrounded;

        if (isMoving && isGrounded)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0)
            {
                PlayRandomClip(footstepClips);

                float speed = characterController.velocity.magnitude;
                stepTimer = speed > 6f ? sprintStepInterval : walkStepInterval;
            }
        }
        else
        {
            stepTimer = 0;
        }
    }

    private void HandleLanding()
    {
        bool isGrounded = characterController.isGrounded;

        // Detect landing
        if (isGrounded && !wasGrounded)
        {
            PlayRandomClip(landClips);
        }

        wasGrounded = isGrounded;
    }

    public void PlayJumpSound()
    {
        PlayRandomClip(jumpClips);
    }

    private void PlayRandomClip(AudioClip[] clips)
    {
        if (clips.Length > 0)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            AudioManager.Instance.PlaySFXWithPitchVariation(clip, pitchVariationMin, pitchVariationMax);
        }
    }
}