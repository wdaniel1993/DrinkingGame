using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using DrinkingGame.BusinessLogic.Models;
using DrinkingGame.BusinessLogic.Transitions;

namespace DrinkingGame.BusinessLogic.States
{
    public class AnswerReading : IState
    {
        private readonly Game _game;

        public AnswerReading(Game game)
        {
            _game = game;
        }


        public IObservable<Transition> Enter()
        {
            return _game.CurrentRound.GuessesAdded.LastOrDefaultAsync().Select(_ => Transition.ToLoserDrinking);
        }
    }
}
