using System;
using System.Collections.Generic;
using System.Text;
using DrinkingGame.BusinessLogic.Models;
using DrinkingGame.BusinessLogic.States;

namespace DrinkingGame.BusinessLogic.Machine
{
    public class StateFactory : IStateFactory
    {
        private readonly Game _game;

        public StateFactory(Game game)
        {
            _game = game;
        }

        public IState GameStarting()
        {
            return new GameStarting();
        }

        public IState Initializing()
        {
            return new Initializing(_game);
        }

        public IState RoundStarting()
        {
            return new RoundStarting(_game);
        }

        public IState AnswerReading()
        {
            return new AnswerReading(_game);
        }

        public IState LoserDrinking()
        {
            return new LoserDrinking(_game);
        }

        public IState RoundEnding()
        {
            return new RoundEnding(_game);;
        }

        public IState GameEnding()
        {
            return new GameEnding(_game);
        }
    }
}
