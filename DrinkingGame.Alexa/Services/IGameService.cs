using System.Collections.Generic;
using DrinkingGame.BusinessLogic.Models;

namespace DrinkingGame.Alexa.Services
{
    public interface IGameService
    {
        IEnumerable<Game> Games { get; }

        int StartNewGame();
    }
}
