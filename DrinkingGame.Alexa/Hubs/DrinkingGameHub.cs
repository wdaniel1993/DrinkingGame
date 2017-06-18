using DrinkingGame.Shared.Interfaces;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac;
using DrinkingGame.BusinessLogic.Machine;
using DrinkingGame.BusinessLogic.Models;
using DrinkingGame.Shared.DataTransfer;
using DrinkingGame.WebService.Handler;
using DrinkingGame.WebService.Services;

namespace DrinkingGame.WebService.Hubs
{
    public class DrinkingGameHub : Hub<IGameClient>, IGameServer
    {
        private readonly IGameService _gameService;

        public DrinkingGameHub(IGameService gameService, IGameHandler handler)
        {
            _gameService = gameService;
            handler.Start();
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
                try
                {
                    await game.PlayerDrank(gamePlayer);
                }
                catch (StateException)
                {}
            }
        }
    }
}