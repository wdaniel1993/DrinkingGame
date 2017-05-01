using DrinkingGame.Shared.Interfaces;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public void ConnectToGame(int gameNumber, bool supportShouldDrink)
        {
            _gameService.Games.SingleOrDefault(x => x.Id == gameNumber)?.AddDevice(new Device()
            {
                ConnectionId = Context.ConnectionId,
                SupportsShouldDrink = supportShouldDrink
            });
        }

        public void GaveAnswer(string player, int answer)
        {
            throw new NotImplementedException();
        }

        public void PlayerDrank(string player)
        {
            throw new NotImplementedException();
        }
    }
}