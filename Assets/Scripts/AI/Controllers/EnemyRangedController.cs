namespace Game.AI.Controllers
{
    using UnityEngine;
    using Game.AI.States;

    /// <summary>
    /// Specific controller for Ranged Enemies.
    /// Configures the State Machine with Ranged-specific parameters, like maintaining distance.
    /// </summary>
    public class EnemyRangedController : BaseAIController
    {
        [Header("Ranged Settings")]
        [SerializeField] private float _attackCooldown = 2f;
        [SerializeField] private int _attackDamage = 10;
        
        [Header("Patrol Settings")]
        [SerializeField] private Transform[] _patrolWaypoints;
        [SerializeField] private float _idleDuration = 3f;

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

            // 1. Initial dummy creation
            IdleState = new AIIdleState(this, _idleDuration, null, null);
            PatrolState = new AIPatrolState(this, _patrolWaypoints, null, null);
            ChaseState = new AIChaseState(this, null, null);
            AttackState = new AIAttackState(this, null, _attackCooldown, _attackDamage, isRanged: true);
            ReturnState = new AIReturnState(this, _homePosition, null, null);

            // 2. Wiring them up recursively
            ReturnState = new AIReturnState(this, _homePosition, IdleState, ChaseState);
            AttackState = new AIAttackState(this, ChaseState, _attackCooldown, _attackDamage, isRanged: true);
            ChaseState = new AIChaseState(this, AttackState, ReturnState);
            PatrolState = new AIPatrolState(this, _patrolWaypoints, IdleState, ChaseState);
            IdleState = new AIIdleState(this, _idleDuration, PatrolState, ChaseState);

            // 3. Final link using established references
            ReturnState = new AIReturnState(this, _homePosition, IdleState, ChaseState);
            AttackState = new AIAttackState(this, ChaseState, _attackCooldown, _attackDamage, isRanged: true);
            ChaseState = new AIChaseState(this, AttackState, ReturnState);
            PatrolState = new AIPatrolState(this, _patrolWaypoints, IdleState, ChaseState);

            // Start the State Machine
            StateMachine.Initialize(IdleState);
        }
    }
}
