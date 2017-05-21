using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DrinkingGame.Shared.Interfaces
{
    public interface IGameServer
    {
        Task ConnectToGame(int gameNumber, bool supportShouldDrink);
        Task PlayerDrank(string player);
        Task GaveAnswer(string player, int answer);
    }
}
