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
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private float _dashForce = 15f;
        [SerializeField] private float _dashDuration = 0.2f;

        [Header("Attack Settings")]
        [SerializeField] private float _attackCooldown = 0.5f;
        [SerializeField] private int _attackDamage = 20;
        [SerializeField] private int _maxAmmo = 30;
        [SerializeField] private LayerMask _attackLayer;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private Transform _shootPoint;

        // Exposed Properties for States
        public float MoveSpeed => _moveSpeed * _speedMultiplier;
        public float RotationSpeed => _rotationSpeed;
        public float DashForce => _dashForce;
        public float DashDuration => _dashDuration;
        public float AttackCooldown => _attackCooldown;
        public int AttackDamage => _attackDamage;
        public int MaxAmmo => _maxAmmo;
        public LayerMask AttackLayer => _attackLayer;
        public GameObject ProjectilePrefab => _projectilePrefab;
        public Transform ShootPoint => _shootPoint;

        public StateMachine StateMachine { get; private set; }
        public Rigidbody Rb { get; private set; }
        public Animator Animator { get; private set; }
        public Health Health { get; private set; }

        // Animation Parameters
        public static readonly int IdleHash = Animator.StringToHash("Idle");
        public static readonly int MoveHash = Animator.StringToHash("Move");
        public static readonly int DashHash = Animator.StringToHash("Dash");
        public static readonly int AttackHash = Animator.StringToHash("Attack");
        public static readonly int DieHash = Animator.StringToHash("Die");

        // Input Data
        public Vector2 MovementInput { get; private set; }
        public bool DashInput { get; private set; }
        public bool AttackInput { get; private set; }

        // Buff State
        private float _speedMultiplier = 1f;
        private float _buffTimer = 0f;
        private int _currentAmmo;

        public int CurrentAmmo => _currentAmmo;

        // State Instances
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerDashState DashState { get; private set; }
        public PlayerAttackState AttackState { get; private set; }
        public PlayerDeathState DeathState { get; private set; }

        private void Awake()
        {
            // Initialize Core Components
            Rb = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();
            Health = GetComponent<Health>();
            _currentAmmo = _maxAmmo;

            // Ensure Rigidbody is configured for clean kinematic/physics hybrid if needed
            Rb.constraints = RigidbodyConstraints.FreezeRotation;

            // Initialize State Machine & States
            StateMachine = new StateMachine();
            
            IdleState = new PlayerIdleState(this);
            MoveState = new PlayerMoveState(this);
            DashState = new PlayerDashState(this);
            AttackState = new PlayerAttackState(this);
            DeathState = new PlayerDeathState(this);
        }

        private void Start()
        {
            StateMachine.Initialize(IdleState);
            // Fire initial UI update
            if (Health != null)
                HandleDamaged(Health.CurrentHealth);
            
            GameEvents.OnAmmoChanged?.Invoke(_currentAmmo, _maxAmmo);
        }

        private void OnEnable()
        {
            if (Health != null)
            {
                Health.OnDamaged.AddListener(HandleDamaged);
                Health.OnDeath.AddListener(HandleDeath);
            }
            GameEvents.OnPlayerHealed += HandleHealed;
            GameEvents.OnPlayerSpeedBuffed += HandleSpeedBuffed;
        }

        private void OnDisable()
        {
            if (Health != null)
            {
                Health.OnDamaged.RemoveListener(HandleDamaged);
                Health.OnDeath.RemoveListener(HandleDeath);
            }
            GameEvents.OnPlayerHealed -= HandleHealed;
            GameEvents.OnPlayerSpeedBuffed -= HandleSpeedBuffed;
        }

        private void Update()
        {
            HandleInput();
            HandleBuffs();
            StateMachine.Update();
        }

        private void HandleBuffs()
        {
            if (_buffTimer > 0f)
            {
                _buffTimer -= Time.deltaTime;
                if (_buffTimer <= 0f)
                {
                    _speedMultiplier = 1f;
                    Debug.Log("[PlayerController] Speed buff ended.");
                }
            }
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
            GameEvents.OnPlayerDamaged?.Invoke(currentHealth, Health.MaxHealth);
            Debug.Log($"[PlayerController] Player took damage. Health: {currentHealth}/{Health.MaxHealth}");
        }

        private void HandleDeath()
        {
            StateMachine.ChangeState(DeathState);
            GameEvents.OnPlayerDied?.Invoke();
            Debug.Log("[PlayerController] Player Died!");
        }

        private void HandleHealed(int amount)
        {
            Health?.Heal(amount);
            Debug.Log($"[PlayerController] Player healed for {amount}. Current HP: {Health.CurrentHealth}");
        }

        private void HandleSpeedBuffed(float multiplier, float duration)
        {
            _speedMultiplier = multiplier;
            _buffTimer = duration;
            Debug.Log($"[PlayerController] Player speed buffed by {multiplier}x for {duration} seconds.");
        }

        public void ConsumeAmmo(int amount = 1)
        {
            _currentAmmo = Mathf.Max(0, _currentAmmo - amount);
            GameEvents.OnAmmoChanged?.Invoke(_currentAmmo, _maxAmmo);
        }

        /// <summary>
        /// Calculates the rotation towards the mouse cursor on a horizontal plane.
        /// </summary>
        public Quaternion GetMouseLookRotation()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, transform.position);
            if (groundPlane.Raycast(ray, out float distance))
            {
                Vector3 targetPoint = ray.GetPoint(distance);
                Vector3 direction = (targetPoint - transform.position);
                direction.y = 0f; // Keep rotation horizontal
                if (direction.sqrMagnitude > 0.01f)
                {
                    return Quaternion.LookRotation(direction);
                }
            }
            return transform.rotation;
        }
    }
}
