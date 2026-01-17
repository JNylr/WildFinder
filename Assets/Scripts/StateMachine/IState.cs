namespace Gear.StateMachine
{
    /// <summary>
    /// Interface for all state implementations.
    /// Used by StateMachine to manage entity behavior.
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Called when entering this state.
        /// Used for initialization and setup.
        /// </summary>
        void Enter();

        /// <summary>
        /// Called every frame while in this state.
        /// Used for state-specific logic and transitions.
        /// </summary>
        void Update();

        /// <summary>
        /// Called at fixed intervals while in this state.
        /// Used for physics-based logic.
        /// </summary>
        void FixedUpdate();

        /// <summary>
        /// Called when exiting this state.
        /// Used for cleanup.
        /// </summary>
        void Exit();
    }
}
