using System;
using System.Collections.Generic;
using System.Text;
using DrinkingGame.BusinessLogic.Transitions;

namespace DrinkingGame.BusinessLogic.States
{
    public interface IState
    {
        IObservable<ITransition> Enter();
    }
}
