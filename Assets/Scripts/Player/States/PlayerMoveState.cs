namespace Game.Player.States
{
    using UnityEngine;
    using Game.Player.Controllers;

    /// <summary>
    /// Player Move state. Handles ground movement via Rigidbody.
    /// </summary>
    public class PlayerMoveState : PlayerBaseState
    {
        public PlayerMoveState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            Debug.Log("[STATE] Entered: Move");
        }

        public override void Update()
        {
            // Transition priorities: Attack > Dash > Idle
            if (_player.AttackInput)
            {
                _player.StateMachine.ChangeState(_player.AttackState);
                return;
            }

            if (_player.DashInput)
            {
                _player.StateMachine.ChangeState(_player.DashState);
                return;
            }

            // Return to idle if no input
            if (_player.MovementInput.sqrMagnitude <= 0.01f)
            {
                _player.StateMachine.ChangeState(_player.IdleState);
                return;
            }
        }

        public override void FixedUpdate()
        {
            // Apply movement ignoring Y to keep gravity intact if jumping is added later
            Vector3 movement = new Vector3(_player.MovementInput.x, 0f, _player.MovementInput.y);
            Vector3 targetVelocity = movement * _player.MoveSpeed;

            // Preserve current Y velocity for gravity
            targetVelocity.y = _player.Rb.linearVelocity.y;

            _player.Rb.linearVelocity = targetVelocity;

            // Rotate towards movement direction
            if (movement.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement);
                _player.transform.rotation = Quaternion.Slerp(
                    _player.transform.rotation, 
                    targetRotation, 
                    Time.fixedDeltaTime * _player.RotationSpeed
                );
            }
        }
    }
}
