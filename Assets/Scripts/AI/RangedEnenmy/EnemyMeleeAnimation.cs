using UnityEngine;

public class EnemyMeleeAnimation : MonoBehaviour
{
    // Singleton instance
    public static EnemyMeleeAnimation Instance;

    public Animator animator;

    // Trigger names
    private readonly string idleTrigger = "Idle";
    private readonly string runTrigger = "Run";
    private readonly string attackTrigger = "Attack";
    private readonly string dieTrigger = "Die";

    void Awake()
    {
        // Singleton setup 
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //animator = GetComponent<Animator>();

        //if (animator == null)
        //{
        //    Debug.LogError("Animator component not found on " + gameObject.name);
        //}
    }

    // Reset all triggers
    private void ResetAllTriggers()
    {
        animator.ResetTrigger(idleTrigger);
        animator.ResetTrigger(runTrigger);
        animator.ResetTrigger(attackTrigger);
        animator.ResetTrigger(dieTrigger);
    }

    public void PlayIdle()
    {
        ResetAllTriggers();
        animator.SetTrigger(idleTrigger);
    }

    public void PlayRun()
    {
        ResetAllTriggers();
        animator.SetTrigger(runTrigger);
    }

    public void PlayAttack()
    {
        ResetAllTriggers();
        animator.SetTrigger(attackTrigger);
    }

    public void PlayDie()
    {
        ResetAllTriggers();
        animator.SetTrigger(dieTrigger);
    }
}