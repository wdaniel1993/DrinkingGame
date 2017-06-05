using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Web;
using DrinkingGame.BusinessLogic.Models;
using DrinkingGame.BusinessLogic.States;
using DrinkingGame.WebService.Communicators;
using DrinkingGame.WebService.Services;

namespace DrinkingGame.WebService.Handler
{
    public class GameHandler : IGameHandler
    {
        private readonly IGameService _gameService;
        private readonly IPuzzleService _puzzleService;
        private readonly IDrinkingGameCommunicator _gameCommunicator;
        private IDisposable _disposable;
        private bool _isActive;

        public bool IsActive => _isActive;

        public GameHandler(IGameService gameService, IPuzzleService puzzleService,IDrinkingGameCommunicator gameCommunicator)
        {
            _gameService = gameService;
            _puzzleService = puzzleService;
            _gameCommunicator = gameCommunicator;
            
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        public void Start()
        {
            if (_isActive)
            {
                return;
            }

            _isActive = true;

            var disposable = new CompositeDisposable();

            disposable.Add(_gameService.GameAdded.Subscribe(
                game =>
                {
                    disposable.Add(game.DeviceAdded.Subscribe(_ => _gameCommunicator.UpdateGameDetails(game)));
                    disposable.Add(game.PlayerAdded.Subscribe(_ => _gameCommunicator.UpdateGameDetails(game)));

                    disposable.Add(game.Machine.State
                        .Do(x =>
                        {
                            Console.WriteLine(x);
                        })
                        .Where(x => x is RoundStarting)
                        .Subscribe(async _ => await game.AddRound(new Round
                        {
                            Game = game,
                            Puzzle = _puzzleService.GetRandomPuzzle()
                        })));

                    disposable.Add(game.RoundAdded.Subscribe(round =>
                    {
                        _gameCommunicator.NewQuestion(game, round.Puzzle.Question);
                        disposable.Add(round.GuessesAdded.Subscribe(guess =>
                        {
                            _gameCommunicator.NewAnswer(game, round.Puzzle.Question, guess.Player.Name, guess.Estimate);
                        }));
                        disposable.Add(round.RoundCompleted.Subscribe(losers =>
                        {
                            _gameCommunicator.CorrectAnswer(game, round.Puzzle.Question, round.Puzzle.Answer);
                            _gameCommunicator.ShouldDrink(game, losers.Select(x => x.Name));
                            _gameCommunicator.UpdateScores(game);
                        }));
                    }));

                }
            ));

            _disposable = disposable;
        }
    }
}