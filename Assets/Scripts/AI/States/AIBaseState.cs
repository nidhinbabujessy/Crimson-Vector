namespace Game.AI.States
{
    using UnityEngine;
    using Game.AI.Controllers;

    /// <summary>
    /// Abstract base state for all AI states.
    /// Holds a reference to the BaseAIController for easy access to shared data.
    /// </summary>
    public abstract class AIBaseState : Game.Core.StateMachine.IState
    {
        protected readonly BaseAIController _ai;

        protected AIBaseState(BaseAIController ai)
        {
            _ai = ai;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }
}
