using System;
using System.Collections.Generic;
using System.Text;

namespace DrinkingGame.Shared.Interfaces
{
    public interface IGameServer
    {
        void ConnectToGame(int gameNumber, bool supportShouldDrink);
        void PlayerDrank(string player);
        void GaveAnswer(string player, int answer);
    }
}
