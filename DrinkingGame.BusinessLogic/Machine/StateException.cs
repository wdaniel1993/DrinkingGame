using System;
using System.Collections.Generic;
using System.Text;
using DrinkingGame.BusinessLogic.States;

namespace DrinkingGame.BusinessLogic.Machine
{
    public class StateException : Exception
    {
        public Type CurrentState { get; }

        public Type[] ExpectedStates { get; }

        public StateException(Type currentState, params Type[] expectedStates)
        {
            CurrentState = currentState;
            ExpectedStates = expectedStates;
        }
    }
}
