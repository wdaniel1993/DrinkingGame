using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrinkingGame.BusinessLogic.Models;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace DrinkingGame.Alexa.Communicators
{
    public class DrinkingGameCommunicator : IDrinkingGameCommunicator
    {
        public IConnectionManager ConnectionManager { get; set; }

        public void NewAnswer(Game game, string question, string player)
        {
            throw new NotImplementedException();
        }

        public void NewQuestion(Game game, string question)
        {
            throw new NotImplementedException();
        }

        public void ShouldDrink(string player)
        {
            throw new NotImplementedException();
        }

        public void UpdateGameDetails(Game game)
        {
            throw new NotImplementedException();
        }

        public void UpdateScores(Game game)
        {
            throw new NotImplementedException();
        }
    }
}