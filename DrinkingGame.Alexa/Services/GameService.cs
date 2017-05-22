using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrinkingGame.BusinessLogic.Models;

namespace DrinkingGame.WebService.Services
{
    public class GameService : IGameService
    {
        private readonly List<Game> _games = new List<Game>();
        public IEnumerable<Game> Games => _games.AsReadOnly();
        public int StartNewGame()
        {
            var nextId = _games.Select(x => x.Id).DefaultIfEmpty().Max() + 1;
            _games.Add(new Game(nextId));
            return nextId;
        }
    }
}