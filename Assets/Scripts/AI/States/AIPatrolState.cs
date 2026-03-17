namespace Game.AI.States
{
    using UnityEngine;
    using Game.AI.Controllers;

    /// <summary>
    /// AI Patrol state. Moves between assigned waypoints.
    /// Transitions to Chase if target is detected.
    /// Transitions to Idle when a waypoint is reached.
    /// </summary>
    public class AIPatrolState : AIBaseState
    {
        private readonly Transform[] _waypoints;
        private readonly Game.Core.StateMachine.IState _idleState;
        private readonly Game.Core.StateMachine.IState _chaseState;
        
        private int _currentWaypointIndex;

        public AIPatrolState(BaseAIController ai, Transform[] waypoints, Game.Core.StateMachine.IState idleState, Game.Core.StateMachine.IState chaseState) : base(ai)
        {
            _waypoints = waypoints;
            _idleState = idleState;
            _chaseState = chaseState;
            _currentWaypointIndex = 0;
        }

        public override void Enter()
        {
            if (_waypoints == null || _waypoints.Length == 0)
            {
                _ai.StateMachine.ChangeState(_idleState);
                return;
            }

            _ai.MoveTo(_waypoints[_currentWaypointIndex].position);
        }

        public override void Update()
        {
            if (_ai.IsTargetInDetectionRange() && _chaseState != null)
            {
                _ai.StateMachine.ChangeState(_chaseState);
                return;
            }

            // Check if reached destination
            if (!_ai.Agent.pathPending && _ai.Agent.remainingDistance <= _ai.Agent.stoppingDistance)
            {
                // Move to next waypoint for the next patrol iteration
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                
                if (_idleState != null)
                {
                    _ai.StateMachine.ChangeState(_idleState);
                }
            }
        }
    }
}
