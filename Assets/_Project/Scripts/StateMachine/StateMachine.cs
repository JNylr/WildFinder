using UnityEngine;
using Gear.Core;

namespace Gear.StateMachine
{
    /// <summary>
    /// Generic state machine controller.
    /// Manages state transitions and delegates Update calls.
    /// </summary>
    public class BaseStateMachine : MonoBehaviour
    {
        private IState currentState;

        /// <summary>
        /// Changes to a new state.
        /// Called by states or controllers to transition behavior.
        /// </summary>
        /// <param name="newState">The state to transition to</param>
        public void ChangeState(IState newState)
        {
            if (currentState != null)
            {
                currentState.Exit();
                LogManager.Log($"Exiting state: {currentState.GetType().Name}", this);
            }

            currentState = newState;

            if (currentState != null)
            {
                currentState.Enter();
                LogManager.Log($"Entering state: {currentState.GetType().Name}", this);
            }
        }

        /// <summary>
        /// Gets the current active state.
        /// Called by external systems to query current behavior.
        /// </summary>
        public IState CurrentState => currentState;

        private void Update()
        {
            currentState?.Update();
        }

        private void FixedUpdate()
        {
            currentState?.FixedUpdate();
        }
    }
}
