namespace Game.AI.Controllers
{
    using UnityEngine;
    using UnityEngine.AI;
    using Game.Core.StateMachine;

    /// <summary>
    /// Base class for all AI Controllers.
    /// Handles dependencies (NavMeshAgent, Target Transform) and updates the StateMachine.
    /// Does not contain behavior logic.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class BaseAIController : MonoBehaviour
    {
        [Header("Detection Settings")]
        [SerializeField] protected float _detectionRange = 10f;
        [SerializeField] protected float _attackRange = 2f;
        [SerializeField] protected LayerMask _targetLayer;
        
        public StateMachine StateMachine { get; private set; }
        public NavMeshAgent Agent { get; private set; }
        public Transform Target { get; private set; }

        public float DetectionRange => _detectionRange;
        public float AttackRange => _attackRange;

        protected virtual void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            StateMachine = new StateMachine();
        }

        protected virtual void Start()
        {
            // For Day 3 simplicity, finding the player by tag. 
            // In a larger system, use an Event or a specific LevelManager to assign targets.
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                Target = playerObj.transform;
            }
        }

        protected virtual void Update()
        {
            StateMachine.Update();
        }

        protected virtual void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        // --- Core Movement Coordination API for States --- //
        public void MoveTo(Vector3 position)
        {
            Agent.isStopped = false;
            Agent.SetDestination(position);
        }

        public void StopMovement()
        {
            if (Agent.isOnNavMesh)
            {
                Agent.isStopped = true;
                Agent.ResetPath();
            }
        }

        // --- Shared Detection Logic --- //
        public bool IsTargetInDetectionRange()
        {
            if (Target == null) return false;
            return Vector3.Distance(transform.position, Target.position) <= _detectionRange;
        }

        public bool IsTargetInAttackRange()
        {
            if (Target == null) return false;
            return Vector3.Distance(transform.position, Target.position) <= _attackRange;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _detectionRange);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
}
