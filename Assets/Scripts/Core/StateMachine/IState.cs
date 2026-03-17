namespace Game.Core.StateMachine
{
    /// <summary>
    /// Interface for all states in the state machine.
    /// Used by both Player and AI state machines.
    /// </summary>
    public interface IState
    {
        void Enter();
        void Exit();
        void Update();
        void FixedUpdate();
    }
}
