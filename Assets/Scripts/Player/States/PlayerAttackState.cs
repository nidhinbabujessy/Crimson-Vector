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
            _player.Animator?.SetTrigger(PlayerController.AttackHash);
            _cooldownTimer = _player.AttackCooldown;
            _hasAttacked = false;

            if (_player.CurrentAmmo > 0)
            {
                // Stop movement during attack
                _player.Rb.linearVelocity = new Vector3(0f, _player.Rb.linearVelocity.y, 0f);
                PerformAttack();
            }
            else
            {
                Debug.Log("[PlayerAttackState] Out of ammo!");
                _hasAttacked = true; // Prevent multiple logs/logic
            }
        }

        private void PerformAttack()
        {
            if (_hasAttacked) return;
            _hasAttacked = true;

            if (_player.ProjectilePrefab == null || _player.ShootPoint == null)
            {
                Debug.LogWarning("[PlayerAttackState] Projectile Prefab or Shoot Point is missing!");
                return;
            }

            _player.ConsumeAmmo();
            GameObject projectileObj = Object.Instantiate(_player.ProjectilePrefab, _player.ShootPoint.position, _player.ShootPoint.rotation);
            if (projectileObj.TryGetComponent(out Game.AI.Common.Projectile projectile))
            {
                projectile.Initialize(_player.AttackDamage, _player.AttackLayer, _player.gameObject);
                Debug.Log($"[PlayerAttackState] Projectile initialized with damage: {_player.AttackDamage}");
            }
            else
            {
                Debug.LogError("[PlayerAttackState] Projectile Prefab is missing the Projectile script!");
            }
            
            Debug.Log($"[Attack] Projectile spawned at {_player.ShootPoint.position}");
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

        public override void FixedUpdate()
        {
            // Allow rotating towards mouse while attacking
            Quaternion targetRotation = _player.GetMouseLookRotation();
            _player.transform.rotation = Quaternion.Slerp(
                _player.transform.rotation,
                targetRotation,
                Time.fixedDeltaTime * _player.RotationSpeed
            );
        }
    }
}
