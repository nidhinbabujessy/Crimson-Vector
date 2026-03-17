namespace Game.Player.States
{
    using UnityEngine;
    using Game.Player.Controllers;

    /// <summary>
    /// Player Dash state. Applies a burst of velocity and ignores input for a duration.
    /// </summary>
    public class PlayerDashState : PlayerBaseState
    {
        private float _dashTimer;
        private Vector3 _dashDirection;

        public PlayerDashState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            Debug.Log("[STATE] Entered: Dash");
            _dashTimer = _player.DashDuration;

            // Determine dash direction (fallback to forward if no input)
            if (_player.MovementInput.sqrMagnitude > 0.01f)
            {
                _dashDirection = new Vector3(_player.MovementInput.x, 0f, _player.MovementInput.y).normalized;
            }
            else
            {
                _dashDirection = _player.transform.forward;
            }

            // Apply immediate burst velocity
            _player.Rb.linearVelocity = new Vector3(
                _dashDirection.x * _player.DashForce,
                _player.Rb.linearVelocity.y,
                _dashDirection.z * _player.DashForce
            );
        }

        public override void Update()
        {
            _dashTimer -= Time.deltaTime;

            if (_dashTimer <= 0f)
            {
                // Dash finished, return to move or idle
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
            // Optional: Maintain dash velocity each physics step if damping is an issue
            _player.Rb.linearVelocity = new Vector3(
                _dashDirection.x * _player.DashForce,
                _player.Rb.linearVelocity.y,
                _dashDirection.z * _player.DashForce
            );
        }
    }
}
