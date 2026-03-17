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
    }
}
