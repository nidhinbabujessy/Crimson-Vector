namespace Game.Player.States
{
    using UnityEngine;
    using Game.Player.Controllers;

    /// <summary>
    /// Player Idle state. Handles staying still and looking for input.
    /// </summary>
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            Debug.Log("[STATE] Entered: Idle");
            // Stop movement immediately on entering Idle
            Vector3 currentVelocity = _player.Rb.linearVelocity;
            _player.Rb.linearVelocity = new Vector3(0f, currentVelocity.y, 0f);
        }

        public override void Update()
        {
            // Transition priorities: Attack > Dash > Move
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

            if (_player.MovementInput.sqrMagnitude > 0.01f)
            {
                _player.StateMachine.ChangeState(_player.MoveState);
                return;
            }
        }
        
        public override void FixedUpdate()
        {
            // Continuously ensure we stay stopped while in Idle
            Vector3 currentVelocity = _player.Rb.linearVelocity;
            _player.Rb.linearVelocity = new Vector3(0f, currentVelocity.y, 0f);
        }
    }
}
