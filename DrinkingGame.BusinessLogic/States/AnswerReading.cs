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
            return Observable.Create<Transition>(
                observer => _game.CurrentRound.GuessesAdded.Subscribe(_ => { }, _ => { }, () => observer.OnNext(Transition.ToLoserDrinking))
            );
        }
    }
}
