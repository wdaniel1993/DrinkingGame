using System;
using System.Collections.Generic;
using System.Text;

namespace DrinkingGame.BusinessLogic.Transitions
{
    public enum Transition
    {
        ToGameStarting,
        ToInitializing,
        ToRoundStarting,
        ToAnswerReading,
        ToLoserDrinking,
        ToRoundEnding,
        ToGameEnding
    }
}
