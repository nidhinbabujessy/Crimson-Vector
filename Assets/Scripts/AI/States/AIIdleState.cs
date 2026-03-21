namespace Game.AI.States
{
    using UnityEngine;
    using Game.AI.Controllers;

    /// <summary>
    /// AI Idle state. Stands still and waits for a set duration before patrolling,
    /// or transitions immediately to chase if the target is detected.
    /// </summary>
    public class AIIdleState : AIBaseState
    {
        private Game.Core.StateMachine.IState _patrolState;
        private Game.Core.StateMachine.IState _chaseState;
        private readonly float _idleDuration;
        private float _timer;

        public AIIdleState(BaseAIController ai, float idleDuration) : base(ai)
        {
            _idleDuration = idleDuration;
        }

        public void SetTransitions(Game.Core.StateMachine.IState patrolState, Game.Core.StateMachine.IState chaseState)
        {
            _patrolState = patrolState;
            _chaseState = chaseState;
        }

        public override void Enter()
        {
            _ai.StopMovement();
            _timer = _idleDuration;

            // Trigger Idle animation
            _ai.GetComponent<Common.EnemyAnimation>()?.PlayIdle();
        }

        public override void Update()
        {
            if (_ai.IsTargetInDetectionRange() && _chaseState != null)
            {
                _ai.StateMachine.ChangeState(_chaseState);
                return;
            }

            _timer -= Time.deltaTime;
            if (_timer <= 0f && _patrolState != null)
            {
                _ai.StateMachine.ChangeState(_patrolState);
            }
        }
    }
}
