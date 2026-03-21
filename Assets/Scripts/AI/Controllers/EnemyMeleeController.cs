namespace Game.AI.Controllers
{
    using UnityEngine;
    using Game.AI.States;

    /// <summary>
    /// Specific controller for Melee Enemies.
    /// Configures the State Machine with Melee-specific parameters and states.
    /// </summary>
    public class EnemyMeleeController : BaseAIController
    {
        [Header("Melee Settings")]
        [SerializeField] private float _attackCooldown = 1.5f;
        [SerializeField] private int _attackDamage = 15;
        [SerializeField] private float _idleDuration = 2f;
        
        [Header("Patrol Settings")]
        [SerializeField] private Transform[] _patrolWaypoints;

        private Vector3 _homePosition;

        // States
        public AIIdleState IdleState { get; private set; }
        public AIPatrolState PatrolState { get; private set; }
        public AIChaseState ChaseState { get; private set; }
        public AIAttackState AttackState { get; private set; }
        public AIReturnState ReturnState { get; private set; }

        protected override void Start()
        {
            base.Start();

            _homePosition = transform.position;

            // 1. Initialize states without transitions to prevent circular dependency
            IdleState = new AIIdleState(this, _idleDuration);
            PatrolState = new AIPatrolState(this, _patrolWaypoints);
            ChaseState = new AIChaseState(this);
            AttackState = new AIAttackState(this, _attackCooldown, _attackDamage, isRanged: false);
            ReturnState = new AIReturnState(this, _homePosition);

            // 2. Set Transitions cleanly without creating ghost copies
            ReturnState.SetTransitions(IdleState, ChaseState);
            AttackState.SetTransitions(ChaseState);
            ChaseState.SetTransitions(AttackState, ReturnState);
            PatrolState.SetTransitions(IdleState, ChaseState);
            IdleState.SetTransitions(PatrolState, ChaseState);

            // Start the State Machine
            StateMachine.Initialize(IdleState);
        }
    }
}
