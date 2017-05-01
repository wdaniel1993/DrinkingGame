using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using DrinkingGame.BusinessLogic.Models;
using DrinkingGame.BusinessLogic.Transitions;

namespace DrinkingGame.BusinessLogic.States
{
    public class GameEnding : IState
    {
        private readonly Game _game;

        public GameEnding(Game game)
        {
            _game = game;
        }

        public IObservable<Transition> Enter()
        {
            return Observable.Empty<Transition>();
        }
    }
}
