using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using DrinkingGame.Client.Core.Hubs;
using DrinkingGame.Client.Core.IoT;
using ReactiveUI;
using Splat;
using SuccincT.Options;

namespace DrinkingGame.Client.Core.ViewModels
{
    public class GameViewModel : ReactiveObject
    {
        private readonly DrinkingGameHubProxy _hubProxy;
        private ReactiveList<PlayerViewModel> _players;
        private readonly ObservableAsPropertyHelper<string> _question;
        private readonly ObservableAsPropertyHelper<int> _answer;
        private readonly IList<IAvoidanceSensorService> _sensorServices;

        public ReactiveList<PlayerViewModel> Players => _players;

        public string Question
        {
            get
            {
                return _question.Value;
            }
        }

        public int Answer
        {
            get
            {
                return _answer.Value;
            }
        }

        public GameViewModel(DrinkingGameHubProxy hubProxy = null, IList<IAvoidanceSensorService> sensorServices = null)
        {
            _hubProxy = hubProxy ?? Locator.CurrentMutable.GetService<DrinkingGameHubProxy>();
            _sensorServices = sensorServices ?? Locator.CurrentMutable.GetServices<IAvoidanceSensorService>().ToList();

            _players = new ReactiveList<PlayerViewModel> { ChangeTrackingEnabled = true };

            _hubProxy.UpdateGameDetails
                .Select(x => x.Players.Select(player => new PlayerViewModel { Name = player }).ToList())
                .ObserveOnDispatcher()
                .Subscribe(x =>
            {
                using (_players.SuppressChangeNotifications())
                {
                    _players.Clear();
                    for(var i = 0; i< x.Count; i++)
                    {
                        var player = x[i];
                        player.SensorIndex = i < _sensorServices.Count ? i : default(int?);
                        _players.Add(player);
                    }
                }
            });

            _sensorServices.ToObservable()
                .SelectMany((service, i) => service.Obstacle.Select(obstacle => (obstacle, i)))
                .ObserveOnDispatcher()
                .Subscribe(x =>
                {
                    _players.Where(player => player.SensorIndex == x.Item2)
                        .Select(player =>
                        {
                            player.IsDrinking = !x.Item1;
                            return Unit.Default;
                        });
                });

            _hubProxy.UpdateScores.ObserveOnDispatcher().Subscribe(x =>
            {
                foreach (var entry in x.Scores)
                {
                    _players.TryFirst(pvm => pvm.Name == entry.Key).Map(pvm =>
                    {
                        pvm.Score = entry.Value;
                        pvm.Answer = 0;
                        pvm.ShouldDrink = false;
                        return Unit.Default;
                    });
                }
            });

            _hubProxy.NewRound.Select(x => x.Question).ObserveOnDispatcher().ToProperty(this, pvm => pvm.Question, out _question, "Start game...");

            _hubProxy.ShouldDrink.ObserveOnDispatcher().Subscribe(x =>
            {
                foreach (var player in x.Players)
                {
                    _players.TryFirst(pvm => pvm.Name == player).Map(pvm => pvm.ShouldDrink = true);
                }
            });

            _hubProxy.NewAnswer.ObserveOnDispatcher().Subscribe(x =>
            {
                _players.TryFirst(pvm => pvm.Name == x.Player).Map(pvm => pvm.Answer = x.Answer);
            });

            Observable.Merge(
                _hubProxy.NewRound.Select(_ => 0),
                _hubProxy.CorrectAnswer.Select(x => x.Answer)
            ).ObserveOnDispatcher().ToProperty(this, x => x.Answer, out _answer);
        }


    }
}
