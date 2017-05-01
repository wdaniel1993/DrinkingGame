using System;
using System.Collections.Generic;
using System.Text;
using DrinkingGame.BusinessLogic.States;

namespace DrinkingGame.BusinessLogic.Machine
{
    public class StateException : Exception
    {
        public Type CurrentState { get; }

        public Type ExpectedState { get; }

        public StateException(Type currentState, Type expectedState)
        {
            CurrentState = currentState;
            ExpectedState = expectedState;
        }
    }
}
