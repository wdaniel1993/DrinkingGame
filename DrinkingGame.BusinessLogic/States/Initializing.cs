using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using DrinkingGame.BusinessLogic.Models;
using DrinkingGame.BusinessLogic.Transitions;

namespace DrinkingGame.BusinessLogic.States
{
    public class Initializing : IState
    {
        private readonly Game _game;

        public Initializing(Game game)
        {
            _game = game;
        }

        public IObservable<Transition> Enter()
        {
            return _game.PlayerAdded.LastAsync().Select(_ => (Transition.ToRoundStarting));
        }
    }
}
