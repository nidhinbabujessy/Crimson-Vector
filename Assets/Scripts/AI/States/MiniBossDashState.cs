namespace Game.AI.States
{
    using UnityEngine;
    using Game.AI.Controllers;

    /// <summary>
    /// Special attack for Mini Boss: Dash towards the player.
    /// </summary>
    public class MiniBossDashState : AIBaseState
    {
        private const float DashSpeed = 15f;
        private const float DashDuration = 0.5f;
        private float _timer;
        private Vector3 _dashDirection;

        public MiniBossDashState(BaseAIController ai) : base(ai)
        {
        }

        public override void Enter()
        {
            if (_ai.Target == null) return;

            _timer = DashDuration;
            _dashDirection = (_ai.Target.position - _ai.transform.position).normalized;
            _dashDirection.y = 0;

            _ai.StopMovement();
            _ai.GetComponent<Common.EnemyAnimation>()?.PlayRun(); // Dash typically looks like a fast run
            Debug.Log($"[{_ai.gameObject.name}] Dashing towards player!");
        }

        public override void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer > 0)
            {
                _ai.transform.position += _dashDirection * DashSpeed * Time.deltaTime;
            }
            else
            {
                // Dash finished, return to chase or attack
                if (_ai is MiniBossController boss)
                {
                    _ai.StateMachine.ChangeState(boss.Phase2Chase);
                }
            }
        }
    }
}
