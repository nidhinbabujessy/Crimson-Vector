namespace Game.Player.States
{
    using UnityEngine;
    using Game.Player.Controllers;

    /// <summary>
    /// Player Death state. Plays animation and disables movement.
    /// </summary>
    public class PlayerDeathState : PlayerBaseState
    {
        public PlayerDeathState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            Debug.Log("[STATE] Entered: Death");

            // Stop all movement
            _player.Rb.linearVelocity = Vector3.zero;
            _player.Rb.isKinematic = true;

            // Play Die animation
            _player.Animator?.SetTrigger(PlayerController.DieHash);
        }

        public override void Update()
        {
            // No transitions out of death
        }
    }
}
