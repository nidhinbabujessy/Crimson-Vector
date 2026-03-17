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
        private readonly Game.Core.StateMachine.IState _idleState;
        private readonly Game.Core.StateMachine.IState _chaseState;
        private readonly Vector3 _homePosition;

        public AIReturnState(BaseAIController ai, Vector3 homePosition, Game.Core.StateMachine.IState idleState, Game.Core.StateMachine.IState chaseState) : base(ai)
        {
            _homePosition = homePosition;
            _idleState = idleState;
            _chaseState = chaseState;
        }

        public override void Enter()
        {
            _ai.MoveTo(_homePosition);
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
