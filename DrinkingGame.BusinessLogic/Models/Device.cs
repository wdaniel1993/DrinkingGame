using System;
using System.Collections.Generic;
using System.Linq;

namespace DrinkingGame.Alexa.Models
{
    public class Device
    {
        public string ConnectionId { get; set; }
        public bool SupportsShouldDrink { get; set; }

        public int Game { get; set; }
    }
}