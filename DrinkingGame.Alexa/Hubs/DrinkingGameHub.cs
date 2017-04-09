using DrinkingGame.Shared.Interfaces;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinkingGame.Alexa.Hubs
{
    public class DrinkingGameHub : Hub<IGameClient>, IGameServer
    {
        public void ConnectToGame(int gameNumber, bool supportShouldDrink)
        {
            throw new NotImplementedException();
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