namespace Game.AI.States
{
    using UnityEngine;
    using Game.AI.Controllers;

    /// <summary>
    /// Phase transition state for the Mini Boss.
    /// Graded with invulnerability and plays a special animation/log.
    /// </summary>
    public class MiniBossPhaseTransitionState : AIBaseState
    {
        private Game.Core.StateMachine.IState _onCompleteState;
        private readonly float _duration;
        private float _timer;

        public MiniBossPhaseTransitionState(BaseAIController ai, float duration) : base(ai)
        {
            _duration = duration;
        }

        public void SetTransitions(Game.Core.StateMachine.IState onCompleteState)
        {
            _onCompleteState = onCompleteState;
        }

        public override void Enter()
        {
            _ai.StopMovement();
            _timer = _duration;
            
            // Set Invulnerability
            if (_ai.Health != null)
            {
                _ai.Health.IsInvulnerable = true;
            }

            // Trigger Transition animation
            _ai.GetComponent<Common.EnemyAnimation>()?.PlayIdle(); // Or a specific transition animation if available
            Debug.Log($"[{_ai.gameObject.name}] Initiating Phase Transition! Invulnerability ACTIVE.");
        }

        public override void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                CompleteTransition();
            }
        }

        private void CompleteTransition()
        {
            // Reset Invulnerability
            if (_ai.Health != null)
            {
                _ai.Health.IsInvulnerable = false;
            }

            if (_ai is MiniBossController boss)
            {
                boss.CompleteTransition();
            }

            if (_onCompleteState != null)
            {
                _ai.StateMachine.ChangeState(_onCompleteState);
            }
        }
    }
}
