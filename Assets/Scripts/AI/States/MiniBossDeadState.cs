namespace Game.AI.States
{
    using UnityEngine;
    using Game.AI.Controllers;

    /// <summary>
    /// Final state for the Mini Boss.
    /// Plays death animation and disables logic.
    /// </summary>
    public class MiniBossDeadState : AIBaseState
    {
        public MiniBossDeadState(BaseAIController ai) : base(ai)
        {
        }

        public override void Enter()
        {
            _ai.StopMovement();
            _ai.Agent.enabled = false;
            
            // Play death animation
            _ai.GetComponent<Common.EnemyAnimation>()?.PlayDie();
            
            // Disable controller to stop further updates
            _ai.enabled = false;
            
            // Trigger victory event
            Game.Core.Events.GameEvents.OnBossDied?.Invoke();
            
            Debug.Log($"[{_ai.gameObject.name}] Mini Boss is DEAD. Disabling all actions.");
        }

        public override void Update()
        {
            // Do nothing in dead state
        }
    }
}
