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
            return new Initializing();
        }

        public IState RoundStarting()
        {
            return new RoundStarting();
        }

        public IState AnswerReading()
        {
            return new AnswerReading();
        }

        public IState LoserDrinking()
        {
            return new LoserDrinking();
        }

        public IState RoundEnding()
        {
            return new RoundEnding();;
        }

        public IState GameEnding()
        {
            return new GameEnding();
        }
    }
}
