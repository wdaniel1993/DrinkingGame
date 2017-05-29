using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Web;
using DrinkingGame.WebService.Communicators;
using DrinkingGame.WebService.Services;

namespace DrinkingGame.WebService.Handler
{
    public class GameHandler : IGameHandler
    {
        private readonly IGameService _gameService;
        private readonly IDrinkingGameCommunicator _gameCommunicator;
        private IDisposable _disposable;
        private bool _isActive;

        public bool IsActive => _isActive;

        public GameHandler(IGameService gameService, IDrinkingGameCommunicator gameCommunicator)
        {
            _gameService = gameService;
            _gameCommunicator = gameCommunicator;
        }

        public void StartHandler()
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

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}