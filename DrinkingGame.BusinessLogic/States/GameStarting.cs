using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using DrinkingGame.BusinessLogic.Transitions;

namespace DrinkingGame.BusinessLogic.States
{
    public class GameStarting : IState
    {
        public IObservable<Transition> Enter()
        {
            return Observable.Return(Transition.ToInitializing);
        }
    }
}
