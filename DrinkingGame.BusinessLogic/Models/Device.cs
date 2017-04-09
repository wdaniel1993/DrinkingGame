using System;
using System.Collections.Generic;
using System.Linq;

namespace DrinkingGame.BusinessLogic.Models
{
    public class Device
    {
        public string ConnectionId { get; set; }
        public bool SupportsShouldDrink { get; set; }
    }
}