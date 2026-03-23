namespace Game.AI.States
{
    using UnityEngine;
    using Game.AI.Controllers;
    using Game.Systems.Health;

    /// <summary>
    /// Special attack for Mini Boss: Deals damage to all targets in a radius.
    /// </summary>
    public class MiniBossAreaAttackState : AIBaseState
    {
        private const float Radius = 5f;
        private const int Damage = 40;
        private const float Delay = 1.5f;
        private float _timer;
        private bool _hasAttacked = false;

        public MiniBossAreaAttackState(BaseAIController ai) : base(ai)
        {
        }

        public override void Enter()
        {
            _ai.StopMovement();
            _timer = Delay;
            _hasAttacked = false;
            
            _ai.GetComponent<Common.EnemyAnimation>()?.PlayAttack();
            Debug.Log($"[{_ai.gameObject.name}] Charging Area Attack! (Wait for {Delay}s)");
        }

        public override void Update()
        {
            _timer -= Time.deltaTime;
            if (!_hasAttacked && _timer <= 0)
            {
                PerformAreaAttack();
            }

            if (_hasAttacked && _timer <= -0.5f) // Slight delay after attack
            {
                if (_ai is MiniBossController boss)
                {
                    _ai.StateMachine.ChangeState(boss.Phase2Chase);
                }
            }
        }

        private void PerformAreaAttack()
        {
            _hasAttacked = true;
            Game.Core.Events.GameEvents.OnBossAreaAttack?.Invoke();
            Debug.Log($"[{_ai.gameObject.name}] BOOM! Area Attack triggered.");
            
            Collider[] hits = Physics.OverlapSphere(_ai.transform.position, Radius, _ai.TargetLayer);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out Health health))
                {
                    health.TakeDamage(Damage);
                    Debug.Log($"[{_ai.gameObject.name}] Hit {hit.name} with Area Attack for {Damage} damage.");
                }
            }
        }
    }
}
