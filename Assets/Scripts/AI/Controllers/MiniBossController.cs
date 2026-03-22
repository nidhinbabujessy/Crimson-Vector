namespace Game.AI.Controllers
{
    using UnityEngine;
    using Game.AI.States;
    using Game.Systems.Health;

    /// <summary>
    /// Advanced Mini Boss Controller with multi-phase state machine.
    /// Handles Intro, Phase 1, Phase Transition (Invulnerability), and Phase 2.
    /// </summary>
    public class MiniBossController : BaseAIController
    {
        [Header("Phase 1 Settings (100% -> 50% HP)")]
        [SerializeField] private float _p1AttackCooldown = 2f;
        [SerializeField] private int _p1AttackDamage = 20;
        [SerializeField] private float _p1Speed = 3.5f;

        [Header("Phase 2 Settings (< 50% HP)")]
        [SerializeField] private float _p2AttackCooldown = 1f;
        [SerializeField] private int _p2AttackDamage = 30;
        [SerializeField] private float _p2Speed = 5.5f;

        [Header("Transition Settings")]
        [SerializeField] private float _transitionDuration = 3f;

        private bool _isPhase2 = false;
        private bool _isTransitioning = false;

        // States
        public MiniBossIntroState IntroState { get; private set; }
        public AIChaseState Phase1Chase { get; private set; }
        public AIAttackState Phase1Attack { get; private set; }
        public MiniBossPhaseTransitionState TransitionState { get; private set; }
        public AIChaseState Phase2Chase { get; private set; }
        public AIAttackState Phase2Attack { get; private set; }
        public MiniBossDashState DashState { get; private set; }
        public MiniBossAreaAttackState AreaAttackState { get; private set; }
        public MiniBossDeadState DeadState { get; private set; }

        protected override void Start()
        {
            base.Start();

            // 1. Initialize States
            IntroState = new MiniBossIntroState(this);
            
            Phase1Chase = new AIChaseState(this);
            Phase1Attack = new AIAttackState(this, _p1AttackCooldown, _p1AttackDamage, isRanged: false);
            
            TransitionState = new MiniBossPhaseTransitionState(this, _transitionDuration);
            
            Phase2Chase = new AIChaseState(this);
            Phase2Attack = new MiniBossPhase2AttackState(this, _p2AttackCooldown, _p2AttackDamage);
            
            DashState = new MiniBossDashState(this);
            AreaAttackState = new MiniBossAreaAttackState(this);
            DeadState = new MiniBossDeadState(this);

            // 2. Set Transitions
            IntroState.SetTransitions(Phase1Chase);
            
            Phase1Chase.SetTransitions(Phase1Attack, null);
            Phase1Attack.SetTransitions(Phase1Chase);
            
            TransitionState.SetTransitions(Phase2Chase);
            
            Phase2Chase.SetTransitions(Phase2Attack, null);
            Phase2Attack.SetTransitions(Phase2Chase);
            
            // Note: Dash and Area attacks can be integrated into Phase 2 logic 
            // either via state transitions or handled inside a specialized Phase 2 Combat State.
            // For simplicity, we'll keep them available for the State Machine to switch to.

            // 3. Health listeners
            Health.OnDamaged.AddListener(CheckPhaseTransition);

            // 4. Start in Intro State
            StateMachine.Initialize(IntroState);
            
            Agent.speed = _p1Speed;
        }

        private void OnDestroy()
        {
            if (Health != null)
            {
                Health.OnDamaged.RemoveListener(CheckPhaseTransition);
            }
        }

        private void CheckPhaseTransition(int currentHealth)
        {
            if (!_isPhase2 && !_isTransitioning && currentHealth <= Health.MaxHealth / 2)
            {
                _isTransitioning = true;
                StateMachine.ChangeState(TransitionState);
            }
        }

        public void CompleteTransition()
        {
            _isTransitioning = false;
            _isPhase2 = true;
            Agent.speed = _p2Speed;
            Debug.Log($"[{gameObject.name}] Phase 2 Active!");
        }

        protected override void HandleDeath()
        {
            // Transition to actual dead state instead of just base logic
            StateMachine.ChangeState(DeadState);
        }
    }
}
