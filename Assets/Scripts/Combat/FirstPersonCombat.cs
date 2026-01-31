using Unity.VisualScripting;
using UnityEngine;

public class FirstPersonCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Animator primaryAnimator;
    [SerializeField] private Animator secondaryAnimator;
    

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
        }
        else
        {
            // Release
        }
    }
}
