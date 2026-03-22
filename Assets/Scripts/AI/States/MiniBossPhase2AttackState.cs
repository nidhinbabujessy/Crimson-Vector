namespace Game.AI.States
{
    using UnityEngine;
    using Game.AI.Controllers;

    /// <summary>
    /// Specialized attack state for Phase 2 that can transition to special attacks.
    /// </summary>
    public class MiniBossPhase2AttackState : AIAttackState
    {
        private readonly float _specialAttackChance = 0.3f;
        private float _specialAttackCooldown = 5f;
        private float _specialAttackTimer = 0f;

        public MiniBossPhase2AttackState(BaseAIController ai, float attackCooldown, int damage) 
            : base(ai, attackCooldown, damage, isRanged: true)
        {
        }

        public override void Update()
        {
            base.Update();

            _specialAttackTimer -= Time.deltaTime;

            // Occasionally switch to a special attack if cooldown is ready
            if (_specialAttackTimer <= 0f && _ai.IsTargetInAttackRange())
            {
                if (Random.value < _specialAttackChance)
                {
                    TriggerSpecialAttack();
                }
                else
                {
                    _specialAttackTimer = 2f; // Try again in 2 seconds if chance failed
                }
            }
        }

        private void TriggerSpecialAttack()
        {
            _specialAttackTimer = _specialAttackCooldown;
            
            if (_ai is MiniBossController boss)
            {
                // Randomly choose between Dash and Area Attack
                if (Random.value < 0.5f)
                {
                    _ai.StateMachine.ChangeState(boss.DashState);
                }
                else
                {
                    _ai.StateMachine.ChangeState(boss.AreaAttackState);
                }
            }
        }
    }
}
