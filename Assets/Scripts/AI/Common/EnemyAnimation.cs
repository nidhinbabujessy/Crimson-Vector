using UnityEngine;

namespace Game.AI.Common
{
    /// <summary>
    /// Unified animation controller for all AI types.
    /// Attached directly to the enemy GameObject (or child with Animator).
    /// </summary>
    public class EnemyAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private static readonly int IdleHash = Animator.StringToHash("Idle");
        private static readonly int RunHash = Animator.StringToHash("Run");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int DieHash = Animator.StringToHash("Die");

        private void Awake()
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
                if (_animator == null)
                {
                    _animator = GetComponentInChildren<Animator>();
                }
            }
        }

        private void ResetAllTriggers()
        {
            if (_animator == null) return;
            _animator.ResetTrigger(IdleHash);
            _animator.ResetTrigger(RunHash);
            _animator.ResetTrigger(AttackHash);
            _animator.ResetTrigger(DieHash);
        }

        public void PlayIdle()
        {
            ResetAllTriggers();
            _animator?.SetTrigger(IdleHash);
        }

        public void PlayRun()
        {
            ResetAllTriggers();
            _animator?.SetTrigger(RunHash);
        }

        public void PlayAttack()
        {
            ResetAllTriggers();
            _animator?.SetTrigger(AttackHash);
        }

        public void PlayDie()
        {
            ResetAllTriggers();
            _animator?.SetTrigger(DieHash);
        }
    }
}
