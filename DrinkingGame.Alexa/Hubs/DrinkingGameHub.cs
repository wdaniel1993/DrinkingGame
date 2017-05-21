using DrinkingGame.Shared.Interfaces;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DrinkingGame.Alexa.Services;
using DrinkingGame.BusinessLogic.Models;
using DrinkingGame.Shared.DataTransfer;

namespace DrinkingGame.Alexa.Hubs
{
    public class DrinkingGameHub : Hub<IGameClient>, IGameServer
    {
        private readonly IGameService _gameService;

        public DrinkingGameHub(IGameService gameService)
        {
            _gameService = gameService;
        }

        public async Task ConnectToGame(ConnectToGameDto dto)
        {
            var game = _gameService.Games.SingleOrDefault(x => x.Id == dto.GameNumber);
            if (game != null)
            {
                await game.AddDevice(new Device()
                {
                    ConnectionId = Context.ConnectionId,
                    SupportsShouldDrink = dto.SupportShouldDrink
                });
            }
        }

        public async Task GaveAnswer(GaveAnswerDto dto)
        {
            var game = _gameService.Games.SingleOrDefault(x => x.Devices.Select(device => device.ConnectionId)
                .Contains(Context.ConnectionId));

            var gamePlayer = game?.Players.SingleOrDefault(x => x.Name == dto.Player);

            if (game != null && gamePlayer != null)
            {
                await game.AddGuess(
                    new Guess()
                    {
                        Estimate = dto.Answer,
                        Player = gamePlayer
                    }
                );
            }
        }

        public async Task PlayerDrank(PlayerDrankDto dto)
        {
            var game = _gameService.Games.SingleOrDefault(x => x.Devices.Select(device => device.ConnectionId)
                .Contains(Context.ConnectionId));

            var gamePlayer = game?.Players.SingleOrDefault(x => x.Name == dto.Player);

            if (game != null && gamePlayer != null)
            {
                await game.PlayerDrank(gamePlayer);
            }
        }
    }
}