namespace Game.Player.States
{
    using UnityEngine;
    using Game.Player.Controllers;
    using Game.Systems.Health;

    /// <summary>
    /// Player Attack state. Performs a raycast attack and waits for cooldown.
    /// </summary>
    public class PlayerAttackState : PlayerBaseState
    {
        private float _cooldownTimer;
        private bool _hasAttacked;

        public PlayerAttackState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            Debug.Log("[STATE] Entered: Attack");
            _cooldownTimer = _player.AttackCooldown;
            _hasAttacked = false;

            // Stop movement during attack
            _player.Rb.linearVelocity = new Vector3(0f, _player.Rb.linearVelocity.y, 0f);
            
            PerformAttack();
        }

        private void PerformAttack()
        {
            if (_hasAttacked) return;
            _hasAttacked = true;

            Vector3 origin = _player.transform.position + Vector3.up * 1f; // Slightly elevated
            Vector3 direction = _player.transform.forward;

            // Debug visualizer
            Debug.DrawRay(origin, direction * _player.AttackRange, Color.red, 1f);

            if (Physics.Raycast(origin, direction, out RaycastHit hit, _player.AttackRange, _player.AttackLayer))
            {
                Debug.Log($"[Attack] Hit: {hit.collider.name}");

                // Decoupled damage interaction
                if (hit.collider.TryGetComponent(out Health targetHealth))
                {
                    targetHealth.TakeDamage(_player.AttackDamage);
                }
            }
        }

        public override void Update()
        {
            _cooldownTimer -= Time.deltaTime;

            if (_cooldownTimer <= 0f)
            {
                // Attack finished, evaluate next state
                if (_player.MovementInput.sqrMagnitude > 0.01f)
                {
                    _player.StateMachine.ChangeState(_player.MoveState);
                }
                else
                {
                    _player.StateMachine.ChangeState(_player.IdleState);
                }
            }
        }
    }
}
