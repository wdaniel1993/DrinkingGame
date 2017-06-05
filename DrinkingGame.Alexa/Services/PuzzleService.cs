using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrinkingGame.BusinessLogic.Models;

namespace DrinkingGame.WebService.Services
{
    public class PuzzleService : IPuzzleService
    {
        private readonly List<Puzzle> _puzzles;
        private readonly Random _random;

        public PuzzleService()
        {
            _puzzles = new List<Puzzle>
            {
                new Puzzle {Question = "How many days are a week?", Answer = 7},
                new Puzzle {Question = "How many million people live in Austria?", Answer = 7},
                new Puzzle {Question = "how big is the eiffel tower in  meters?", Answer = 300},
                new Puzzle {Question = "how much tons does a blue whale weigh?", Answer = 17}
            };
            _random = new Random();
        }

        public Puzzle GetRandomPuzzle()
        {
            return _puzzles[_random.Next(_puzzles.Count)];
        }

        public IEnumerable<Puzzle> Puzzles => _puzzles;
    }
}