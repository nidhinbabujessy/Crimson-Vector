namespace Game.AI.States
{
    using UnityEngine;
    using Game.AI.Controllers;

    /// <summary>
    /// AI Return state. Walks back to the start position/patrol area 
    /// if the target escapes detection altogether.
    /// </summary>
    public class AIReturnState : AIBaseState
    {
        private Game.Core.StateMachine.IState _idleState;
        private Game.Core.StateMachine.IState _chaseState;
        private readonly Vector3 _homePosition;

        public AIReturnState(BaseAIController ai, Vector3 homePosition) : base(ai)
        {
            _homePosition = homePosition;
        }

        public void SetTransitions(Game.Core.StateMachine.IState idleState, Game.Core.StateMachine.IState chaseState)
        {
            _idleState = idleState;
            _chaseState = chaseState;
        }

        public override void Enter()
        {
            _ai.MoveTo(_homePosition);

            // Trigger Run animation if this is a Melee enemy
            if (_ai is EnemyMeleeController && EnemyMeleeAnimation.Instance != null)
            {
                EnemyMeleeAnimation.Instance.PlayRun();
            }
        }

        public override void Update()
        {
            // If target comes back within range while returning, chase them again
            if (_ai.IsTargetInDetectionRange() && _chaseState != null)
            {
                _ai.StateMachine.ChangeState(_chaseState);
                return;
            }

            // Check if reached destination
            if (!_ai.Agent.pathPending && _ai.Agent.remainingDistance <= _ai.Agent.stoppingDistance)
            {
                if (_idleState != null)
                {
                    _ai.StateMachine.ChangeState(_idleState);
                }
            }
        }
    }
}
