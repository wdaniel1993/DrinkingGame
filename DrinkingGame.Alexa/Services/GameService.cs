using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Web;
using DrinkingGame.BusinessLogic.Models;

namespace DrinkingGame.WebService.Services
{
    public class GameService : IGameService
    {
        private readonly List<Game> _games = new List<Game>();
        private readonly Subject<Game> _gameAdded = new Subject<Game>();
        public IEnumerable<Game> Games => _games.AsReadOnly();
        public IObservable<Game> GameAdded => _gameAdded.AsObservable();

        public int StartNewGame()
        {
            var nextId = _games.Select(x => x.Id).DefaultIfEmpty().Max() + 1;
            _games.Add(new Game(nextId));
            return nextId;
        }
    }
}