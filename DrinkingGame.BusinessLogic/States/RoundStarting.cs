using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using DrinkingGame.BusinessLogic.Models;
using DrinkingGame.BusinessLogic.Transitions;

namespace DrinkingGame.BusinessLogic.States
{
    public class RoundStarting : IState
    {
        private readonly Game _game;

        public RoundStarting(Game game)
        {
            _game = game;
        }

        public IObservable<Transition> Enter()
        {
            return _game.RoundAdded.Select(_ => Transition.ToAnswerReading);
        }
    }
}
