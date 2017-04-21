using System;
using System.Collections.Generic;
using System.Text;

namespace DrinkingGame.Shared.Interfaces
{
    public interface IGameClient
    {
        void UpdateGameDetails(IEnumerable<string> players);
        void NewQuestion(string question);
        void NewAnswer(string question, string player, int answer);
        void CorrectAnswer(string question, int answer);
        void ShouldDrink(string player);
        void UpdateScores(Dictionary<string,int> scores);
    }
}
