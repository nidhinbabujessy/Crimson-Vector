namespace Game.Player.States
{
    using Game.Core.StateMachine;
    using Game.Player.Controllers;

    /// <summary>
    /// Abstract base state for all player states.
    /// Holds a reference to the PlayerController for easy access to dependencies.
    /// </summary>
    public abstract class PlayerBaseState : IState
    {
        protected readonly PlayerController _player;

        protected PlayerBaseState(PlayerController player)
        {
            _player = player;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }
}
