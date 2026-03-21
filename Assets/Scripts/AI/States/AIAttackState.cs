namespace Game.AI.States
{
    using UnityEngine;
    using Game.AI.Controllers;
    using Game.Systems.Health;

    /// <summary>
    /// AI Attack state. Stops moving and performs an attack logic (Melee/Ranged).
    /// </summary>
    public class AIAttackState : AIBaseState
    {
        private Game.Core.StateMachine.IState _chaseState;
        private readonly float _attackCooldown;
        private readonly int _damage;
        private readonly bool _isRanged;
        
        private float _cooldownTimer;

        public AIAttackState(BaseAIController ai, float attackCooldown, int damage, bool isRanged = false) : base(ai)
        {
            _attackCooldown = attackCooldown;
            _damage = damage;
            _isRanged = isRanged;
        }

        public void SetTransitions(Game.Core.StateMachine.IState chaseState)
        {
            _chaseState = chaseState;
        }

        public override void Enter()
        {
            _ai.StopMovement();
            _cooldownTimer = _attackCooldown / 2f; // Slight delay before first hit
        }

        public override void Update()
        {
            // Always face target
            if (_ai.Target != null)
            {
                Vector3 direction = (_ai.Target.position - _ai.transform.position).normalized;
                direction.y = 0; // Keep rotation strictly horizontal
                if (direction != Vector3.zero)
                {
                    _ai.transform.rotation = Quaternion.Slerp(_ai.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
                }
            }

            // Handle Attack logic
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0f)
            {
                PerformAttack();
                _cooldownTimer = _attackCooldown;
            }

            // Target ran away!
            if (!_ai.IsTargetInAttackRange() && _chaseState != null)
            {
                _ai.StateMachine.ChangeState(_chaseState);
            }
        }

        private void PerformAttack()
        {
            if (_ai.Target == null) return;
            
            // Trigger animation
            if (EnemyMeleeAnimation.Instance != null && !_isRanged)
            {
                EnemyMeleeAnimation.Instance.PlayAttack();
            }

            if (_isRanged)
            {
                // Ranged: Simple Raycast Example (Can be replaced with Projectile later)
                Debug.DrawRay(_ai.transform.position + Vector3.up, _ai.transform.forward * _ai.AttackRange, Color.red, 0.5f);
                if (Physics.Raycast(_ai.transform.position + Vector3.up, _ai.transform.forward, out RaycastHit hit, _ai.AttackRange))
                {
                    if (hit.transform == _ai.Target && hit.transform.TryGetComponent(out Health health))
                    {
                        health.TakeDamage(_damage);
                        Debug.Log($"[{_ai.gameObject.name}] Shot {_ai.Target.name} for {_damage} damage.");
                    }
                }
            }
            else
            {
                // Melee: Direct application assuming target is still in range (verified by Update)
                if (_ai.Target.TryGetComponent(out Health health))
                {
                    health.TakeDamage(_damage);
                    Debug.Log($"[{_ai.gameObject.name}] Melee Attacked {_ai.Target.name} for {_damage} damage.");
                }
            }
        }
    }
}
