using System;
using System.Collections.Generic;
using DrinkingGame.BusinessLogic.Models;

namespace DrinkingGame.WebService.Services
{
    public interface IGameService
    {
        IEnumerable<Game> Games { get; }
        IObservable<Game> GameAdded { get; }

        int StartNewGame(string language);
    }
}
