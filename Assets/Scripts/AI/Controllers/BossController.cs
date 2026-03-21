namespace Game.AI.Controllers
{
    using UnityEngine;
    using Game.AI.States;
    using Game.Systems.Health;

    /// <summary>
    /// Mini Boss Controller with 2 phases.
    /// Phase limits are based on Health. Uses shared AI States but configures them for phases.
    /// </summary>
    [RequireComponent(typeof(Health))]
    public class BossController : BaseAIController
    {
        [Header("Phase 1 Settings (100% -> 50% HP)")]
        [SerializeField] private float _p1AttackCooldown = 2f;
        [SerializeField] private int _p1AttackDamage = 20;

        [Header("Phase 2 Settings (< 50% HP)")]
        [SerializeField] private float _p2AttackCooldown = 0.8f;
        [SerializeField] private int _p2AttackDamage = 35;
        [SerializeField] private float _p2SpeedMultiplier = 1.5f;

        public Health Health { get; private set; }
        private bool _isPhase2 = false;

        // Phase 1 States
        public AIIdleState IdleState { get; private set; }
        public AIChaseState Phase1Chase { get; private set; }
        public AIAttackState Phase1Attack { get; private set; }

        // Phase 2 States (Faster & Ranged)
        public AIChaseState Phase2Chase { get; private set; }
        public AIAttackState Phase2Attack { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Health = GetComponent<Health>();
        }

        protected override void Start()
        {
            base.Start();

            // 1. Initialize states without transitions
            Phase2Chase = new AIChaseState(this);
            Phase2Attack = new AIAttackState(this, _p2AttackCooldown, _p2AttackDamage, isRanged: true);

            Phase1Chase = new AIChaseState(this);
            Phase1Attack = new AIAttackState(this, _p1AttackCooldown, _p1AttackDamage, isRanged: false);
            
            IdleState = new AIIdleState(this, 1f);

            // 2. Set Transitions cleanly
            Phase2Chase.SetTransitions(Phase2Attack, null);
            Phase2Attack.SetTransitions(Phase2Chase);

            Phase1Chase.SetTransitions(Phase1Attack, null);
            Phase1Attack.SetTransitions(Phase1Chase);

            IdleState.SetTransitions(null, Phase1Chase);

            // Subscribe to Health events for Phase transitioning
            Health.OnDamaged.AddListener(HandleDamaged);

            StateMachine.Initialize(IdleState);
        }

        private void OnDestroy()
        {
            if (Health != null)
            {
                Health.OnDamaged.RemoveListener(HandleDamaged);
            }
        }

        private void HandleDamaged(int currentHealth)
        {
            if (!_isPhase2 && currentHealth <= Health.MaxHealth / 2)
            {
                EnterPhase2();
            }
        }

        private void EnterPhase2()
        {
            _isPhase2 = true;
            Debug.Log($"[BossController] {gameObject.name} entered Phase 2! (HP <= 50%)");

            // Visually or mechanically boost boss
            Agent.speed *= _p2SpeedMultiplier;

            // Swiftly transition into phase 2 state depending on range
            if (IsTargetInAttackRange())
            {
                StateMachine.ChangeState(Phase2Attack);
            }
            else
            {
                StateMachine.ChangeState(Phase2Chase);
            }
        }
    }
}
