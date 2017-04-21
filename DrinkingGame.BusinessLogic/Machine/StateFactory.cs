using System;
using System.Collections.Generic;
using System.Text;
using DrinkingGame.BusinessLogic.States;

namespace DrinkingGame.BusinessLogic.Machine
{
    public class StateFactory : IStateFactory
    {
        public IState GameStarting()
        {
            throw new NotImplementedException();
        }

        public IState Initializing()
        {
            throw new NotImplementedException();
        }

        public IState RoundStarting()
        {
            throw new NotImplementedException();
        }

        public IState AnswerReading()
        {
            throw new NotImplementedException();
        }

        public IState LoserDrinking()
        {
            throw new NotImplementedException();
        }

        public IState RoundEnding()
        {
            throw new NotImplementedException();
        }

        public IState GameEnding()
        {
            throw new NotImplementedException();
        }
    }
}
