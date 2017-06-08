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
    public class LoserDrinking : IState
    {
        private readonly Game _game;

        public LoserDrinking(Game game)
        {
            _game = game;
        }

        public IObservable<Transition> Enter()
        {
            if (_game.Devices.Any(x => x.SupportsShouldDrink))
            {
                return _game.CurrentRound.DrinkTaken.LastAsync().Select(_ => Transition.ToRoundEnding);
            }
            else
            {
                return Observable.Return(Transition.ToRoundEnding);
            }
        }
    }
}
