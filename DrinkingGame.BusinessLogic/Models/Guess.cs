﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DrinkingGame.BusinessLogic.Models
{
    [Serializable]
    public class Guess
    {
        public Player Player { get; set; }
        public int Estimate { get; set; }
    }
}
