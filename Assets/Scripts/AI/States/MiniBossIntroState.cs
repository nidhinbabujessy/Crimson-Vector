namespace Game.AI.States
{
    using UnityEngine;
    using Game.AI.Controllers;

    /// <summary>
    /// Initial state for the Mini Boss. Stands still until player enters detection range.
    /// Plays an intro behavior before starting combat.
    /// </summary>
    public class MiniBossIntroState : AIBaseState
    {
        private Game.Core.StateMachine.IState _chaseState;
        private bool _isIntroTriggered = false;

        public MiniBossIntroState(BaseAIController ai) : base(ai)
        {
        }

        public void SetTransitions(Game.Core.StateMachine.IState chaseState)
        {
            _chaseState = chaseState;
        }

        public override void Enter()
        {
            _ai.StopMovement();
            _ai.GetComponent<Common.EnemyAnimation>()?.PlayIdle();
            Debug.Log($"[{_ai.gameObject.name}] Waiting for player to approach...");
        }

        public override void Update()
        {
            if (!_isIntroTriggered && _ai.IsTargetInDetectionRange())
            {
                TriggerIntro();
            }
        }

        private void TriggerIntro()
        {
            _isIntroTriggered = true;
            Debug.Log($"[{_ai.gameObject.name}] ROAR! (Intro Sequence Started)");
            
            // In a real scenario, you might wait for an animation to finish.
            // For now, we transition immediately to combat.
            if (_chaseState != null)
            {
                _ai.StateMachine.ChangeState(_chaseState);
            }
        }
    }
}
