using DrinkingGame.Shared.Interfaces;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DrinkingGame.Alexa.Services;
using DrinkingGame.BusinessLogic.Models;

namespace DrinkingGame.Alexa.Hubs
{
    public class DrinkingGameHub : Hub<IGameClient>, IGameServer
    {
        private readonly IGameService _gameService;

        public DrinkingGameHub(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task ConnectToGame(int gameNumber, bool supportShouldDrink)
        {
            var game = _gameService.Games.SingleOrDefault(x => x.Id == gameNumber);
            if (game != null)
            {
                await game.AddDevice(new Device()
                {
                    ConnectionId = Context.ConnectionId,
                    SupportsShouldDrink = supportShouldDrink
                });
            }
        }

        public async Task GaveAnswer(string player, int answer)
        {
            var game = _gameService.Games.SingleOrDefault(x => x.Devices.Select(device => device.ConnectionId)
                .Contains(Context.ConnectionId));

            var gamePlayer = game?.Players.SingleOrDefault(x => x.Name == player);

            if (game != null && gamePlayer != null)
            {
                await game.AddGuess(
                    new Guess()
                    {
                        Estimate = answer,
                        Player = gamePlayer
                    }
                );
            }
        }

        public async Task PlayerDrank(string player)
        {
            var game = _gameService.Games.SingleOrDefault(x => x.Devices.Select(device => device.ConnectionId)
                .Contains(Context.ConnectionId));

            var gamePlayer = game?.Players.SingleOrDefault(x => x.Name == player);

            if (game != null && gamePlayer != null)
            {
                await game.PlayerDrank(gamePlayer);
            }
        }
    }
}