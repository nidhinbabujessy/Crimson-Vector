namespace Game.AI.States
{
    using UnityEngine;
    using Game.AI.Controllers;

    /// <summary>
    /// AI Chase state. Actively pursues the target using NavMesh.
    /// Transitions to Attack if in range.
    /// Transitions to Return if target is lost.
    /// </summary>
    public class AIChaseState : AIBaseState
    {
        private Game.Core.StateMachine.IState _attackState;
        private Game.Core.StateMachine.IState _returnState;
        
        private const float PathUpdateInterval = 0.2f;
        private float _pathTimer;

        public AIChaseState(BaseAIController ai) : base(ai)
        {
        }

        public void SetTransitions(Game.Core.StateMachine.IState attackState, Game.Core.StateMachine.IState returnState)
        {
            _attackState = attackState;
            _returnState = returnState;
        }

        public override void Enter()
        {
            _pathTimer = 0f;
            
            // Trigger Run animation
            _ai.GetComponent<Common.EnemyAnimation>()?.PlayRun();
        }

        public override void Update()
        {
            // Target lost
            if (!_ai.IsTargetInDetectionRange() && _returnState != null)
            {
                _ai.StateMachine.ChangeState(_returnState);
                return;
            }

            // Target in attack range
            if (_ai.IsTargetInAttackRange() && _attackState != null)
            {
                _ai.StateMachine.ChangeState(_attackState);
                return;
            }

            // Continually update path (inexpensive interval update instead of every frame)
            _pathTimer -= Time.deltaTime;
            if (_pathTimer <= 0f && _ai.Target != null)
            {
                _pathTimer = PathUpdateInterval;
                _ai.MoveTo(_ai.Target.position);
            }
        }
    }
}
