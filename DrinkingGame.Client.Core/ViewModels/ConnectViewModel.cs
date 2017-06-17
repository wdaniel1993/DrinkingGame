using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using DrinkingGame.Client.Core.Hubs;
using DrinkingGame.Shared.DataTransfer;
using Microsoft.AspNet.SignalR.Client;
using ReactiveUI;
using Splat;
using SuccincT.Options;
using SuccincT.Parsers;

namespace DrinkingGame.Client.Core.ViewModels
{
    public class ConnectViewModel : ReactiveObject
    {
        private readonly DrinkingGameHubProxy _hubProxy;
        private readonly HubConnection _connection;
        private readonly ReactiveCommand<Unit, bool> _connectCommand;
        private string _gameId = string.Empty;

        public string GameId
        {
            get => _gameId;
            set => this.RaiseAndSetIfChanged(ref _gameId,value);
        }

        public ReactiveCommand<Unit, bool> ConnectCommand => _connectCommand;

        public ConnectViewModel(DrinkingGameHubProxy hubProxy = null, HubConnection connection = null)
        {
            _hubProxy = hubProxy ?? Locator.CurrentMutable.GetService<DrinkingGameHubProxy>();
            _connection = connection ?? Locator.Current.GetService<HubConnection>();

            var canConnect = this.WhenAny(
                x => x.GameId, 
                (gameId) => !string.IsNullOrWhiteSpace(gameId.Value) && gameId.Value.TryParseInt().HasValue);

            _connectCommand = ReactiveCommand.CreateFromTask(ConnectImpl, canConnect);
        }

        private async Task<bool> ConnectImpl()
        {
            var gameId = GameId?.TryParseInt() ?? Option<int>.None();
            if (_connection.State != ConnectionState.Connected)
            {
                _connection.EnsureReconnecting();
                await _connection.Start();
            }
            
            
            if (gameId.HasValue)
            {
                await _hubProxy.ConnectToGame(new ConnectToGameDto
                {
                    GameNumber = gameId.Value,
                    SupportShouldDrink = true
                });
                return true;
            }
            return false;
        }
    }
}
