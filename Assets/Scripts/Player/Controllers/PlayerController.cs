namespace Game.Player.Controllers
{
    using UnityEngine;
    using Game.Core.StateMachine;
    using Game.Player.States;
    using Game.Systems.Health;
    using Game.Core.Events;

    /// <summary>
    /// The main controller for the Player. 
    /// Responsibilities: Read Input, Expose Physics, Provide State references.
    /// STRICT RULE: No behavior/logic should exist here, only data & routing.
    /// </summary>
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 8f;
        [SerializeField] private float _dashForce = 15f;
        [SerializeField] private float _dashDuration = 0.2f;

        [Header("Attack Settings")]
        [SerializeField] private float _attackRange = 5f;
        [SerializeField] private float _attackCooldown = 0.5f;
        [SerializeField] private int _attackDamage = 20;
        [SerializeField] private LayerMask _attackLayer;

        // Exposed Properties for States
        public float MoveSpeed => _moveSpeed;
        public float DashForce => _dashForce;
        public float DashDuration => _dashDuration;
        public float AttackRange => _attackRange;
        public float AttackCooldown => _attackCooldown;
        public int AttackDamage => _attackDamage;
        public LayerMask AttackLayer => _attackLayer;

        public StateMachine StateMachine { get; private set; }
        public Rigidbody Rb { get; private set; }
        public Health Health { get; private set; }

        // Input Data
        public Vector2 MovementInput { get; private set; }
        public bool DashInput { get; private set; }
        public bool AttackInput { get; private set; }

        // State Instances
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerDashState DashState { get; private set; }
        public PlayerAttackState AttackState { get; private set; }

        private void Awake()
        {
            // Initialize Core Components
            Rb = GetComponent<Rigidbody>();
            Health = GetComponent<Health>();

            // Ensure Rigidbody is configured for clean kinematic/physics hybrid if needed
            Rb.constraints = RigidbodyConstraints.FreezeRotation;

            // Initialize State Machine & States
            StateMachine = new StateMachine();
            
            IdleState = new PlayerIdleState(this);
            MoveState = new PlayerMoveState(this);
            DashState = new PlayerDashState(this);
            AttackState = new PlayerAttackState(this);
        }

        private void Start()
        {
            StateMachine.Initialize(IdleState);
        }

        private void OnEnable()
        {
            if (Health != null)
            {
                Health.OnDamaged.AddListener(HandleDamaged);
                Health.OnDeath.AddListener(HandleDeath);
            }
        }

        private void OnDisable()
        {
            if (Health != null)
            {
                Health.OnDamaged.RemoveListener(HandleDamaged);
                Health.OnDeath.RemoveListener(HandleDeath);
            }
        }

        private void Update()
        {
            HandleInput();
            StateMachine.Update();
        }

        private void FixedUpdate()
        {
            StateMachine.FixedUpdate();
        }

        /// <summary>
        /// Gathers input data for states to read. 
        /// Replace with Input System V2 later if needed.
        /// </summary>
        private void HandleInput()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            MovementInput = new Vector2(horizontal, vertical).normalized;

            DashInput = Input.GetKeyDown(KeyCode.Space);
            AttackInput = Input.GetMouseButtonDown(0);
        }

        // --- Event Handlers ---
        private void HandleDamaged(int currentHealth)
        {
            GameEvents.OnPlayerDamaged?.Invoke(currentHealth);
            Debug.Log($"[PlayerController] Player took damage. Health: {currentHealth}");
        }

        private void HandleDeath()
        {
            GameEvents.OnPlayerDied?.Invoke();
            Debug.Log("[PlayerController] Player Died!");
        }
    }
}
