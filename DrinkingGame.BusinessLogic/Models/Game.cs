using System;
using System.Collections.Generic;
using System.Text;
using DrinkingGame.BusinessLogic.Machine;
using DrinkingGame.BusinessLogic.States;

namespace DrinkingGame.BusinessLogic.Models
{
    public class Game
    {
        public int Id { get; set; }

        public IEnumerable<Player> Players { get; set; }

        public IEnumerable<Device> Devices { get; set; }

        public GameStateMachine Machine { get; set; }
    }
}
