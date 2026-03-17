namespace Game.Core.StateMachine
{
    /// <summary>
    /// A generic, reusable State Machine that handles state transitions and updates.
    /// </summary>
    public class StateMachine
    {
        public IState CurrentState { get; private set; }

        /// <summary>
        /// Initializes the State Machine with a starting state.
        /// </summary>
        public void Initialize(IState startingState)
        {
            CurrentState = startingState;
            CurrentState?.Enter();
        }

        /// <summary>
        /// Changes the active state to a new state.
        /// </summary>
        public void ChangeState(IState newState)
        {
            if (CurrentState == newState) return;

            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState?.Enter();
        }

        /// <summary>
        /// Updates the active state. Should be called in Update().
        /// </summary>
        public void Update()
        {
            CurrentState?.Update();
        }

        /// <summary>
        /// FixedUpdates the active state. Should be called in FixedUpdate().
        /// </summary>
        public void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }
    }
}
