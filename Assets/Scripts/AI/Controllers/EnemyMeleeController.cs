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

            // 1. Initialize states without transitions first to prevent circular dependency
            IdleState = new AIIdleState(this, _idleDuration, null, null);
            PatrolState = new AIPatrolState(this, _patrolWaypoints, null, null);
            ChaseState = new AIChaseState(this, null, null);
            AttackState = new AIAttackState(this, null, _attackCooldown, _attackDamage, isRanged: false);
            ReturnState = new AIReturnState(this, _homePosition, null, null);

            // 2. We use a cleaner approach by passing a Func<IState> or setting properties.
            // Since our constructors require them, let's just re-instantiate them cleanly 
            // once we have the references, or better yet, inject them. 
            // To keep the IState strictly following SOLID and immutable-ish without public setters,
            // we will reconstruct them bottom-up.

            ReturnState = new AIReturnState(this, _homePosition, IdleState, ChaseState);
            AttackState = new AIAttackState(this, ChaseState, _attackCooldown, _attackDamage, isRanged: false);
            ChaseState = new AIChaseState(this, AttackState, ReturnState);
            PatrolState = new AIPatrolState(this, _patrolWaypoints, IdleState, ChaseState);
            IdleState = new AIIdleState(this, _idleDuration, PatrolState, ChaseState);
            
            // Fix the cross-references for the final compiled graph
            ReturnState = new AIReturnState(this, _homePosition, IdleState, ChaseState);
            AttackState = new AIAttackState(this, ChaseState, _attackCooldown, _attackDamage, isRanged: false);
            ChaseState = new AIChaseState(this, AttackState, ReturnState);
            PatrolState = new AIPatrolState(this, _patrolWaypoints, IdleState, ChaseState);

            // Start the State Machine
            StateMachine.Initialize(IdleState);
        }
    }
}
