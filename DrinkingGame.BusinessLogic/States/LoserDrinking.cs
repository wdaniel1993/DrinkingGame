using System;
using System.Collections.Generic;
using System.Text;
using DrinkingGame.BusinessLogic.Transitions;

namespace DrinkingGame.BusinessLogic.States
{
    public class LoserDrinking : IState
    {
        public IObservable<Transition> Enter()
        {
            throw new NotImplementedException();
        }
    }
}
