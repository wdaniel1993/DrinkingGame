using System;
using System.Collections.Generic;
using System.Text;
using DrinkingGame.BusinessLogic.Models;

namespace DrinkingGame.BusinessLogic.Services
{
    public interface IGameService
    {
        IEnumerable<Game> Games { get; }

        int StartNewGame();
    }
}
