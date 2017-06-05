using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using DrinkingGame.BusinessLogic.Models;
using DrinkingGame.BusinessLogic.Transitions;

namespace DrinkingGame.BusinessLogic.States
{
    public class RoundEnding : IState
    {
        private readonly Game _game;

        public RoundEnding(Game game)
        {
            _game = game;
        }

        public IObservable<Transition> Enter()
        {
            var round = _game.CurrentRound;
            return round.RoundCompleted.Select(_ => round.IsLastRound
                ? Transition.ToGameEnding
                : Transition.ToRoundStarting);
        }
    }
}
