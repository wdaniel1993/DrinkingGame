using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrinkingGame.Alexa.Hubs;
using DrinkingGame.BusinessLogic.Models;
using DrinkingGame.Shared.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace DrinkingGame.Alexa.Communicators
{
    public class DrinkingGameCommunicator : IDrinkingGameCommunicator
    {
        private readonly IConnectionManager _connectionManager;

        public DrinkingGameCommunicator(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        private IHubContext<IGameClient> Hub => _connectionManager.GetHubContext<DrinkingGameHub,IGameClient>();

        public void NewAnswer(Game game, string question, string player)
        {
            Hub.Clients.Users(game.Devices.Select(x => x.ConnectionId).ToList()).NewAnswer(question,player);
        }

        public void NewQuestion(Game game, string question)
        {
            Hub.Clients.Users(game.Devices.Select(x => x.ConnectionId).ToList()).NewQuestion(question);
        }

        public void CorrectAnswer(Game game, string question, string answer)
        {
            Hub.Clients.Users(game.Devices.Select(x => x.ConnectionId).ToList()).CorrectAnswer(question, answer);
        }

        public void ShouldDrink(Game game, string player)
        {
            Hub.Clients.Users(game.Devices.Select(x => x.ConnectionId).ToList()).ShouldDrink(player);
        }

        public void UpdateGameDetails(Game game)
        {
            Hub.Clients.Users(game.Devices.Select(x => x.ConnectionId).ToList()).UpdateGameDetails(game.Players.Select(x => x.Name));
        }

        public void UpdateScores(Game game)
        {
            Hub.Clients.Users(game.Devices.Select(x => x.ConnectionId).ToList()).UpdateScores(game.Players.ToDictionary(x => x.Name, y => y.Score));
        }
    }
}