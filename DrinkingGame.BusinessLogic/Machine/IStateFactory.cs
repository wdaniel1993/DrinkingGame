using System;
using System.Collections.Generic;
using System.Text;
using DrinkingGame.BusinessLogic.States;

namespace DrinkingGame.BusinessLogic.Machine
{
    public interface IStateFactory
    {
        IState GameStarting();
        IState Initializing();
        IState RoundStarting();
        IState AnswerReading();
        IState LoserDrinking();
        IState RoundEnding();
        IState GameEnding();
    }
}
