using System;
using System.Collections.Generic;
using System.Text;

namespace DrinkingGame.BusinessLogic.Models
{
    public class Game
    {
        public IEnumerable<Player> Players { get; set; }

        public IEnumerable<Device> Devices { get; set; }
    }
}
