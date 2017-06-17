using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrinkingGame.BusinessLogic.Models;
using Microsoft.Bot.Builder.FormFlow.Advanced;

namespace DrinkingGame.WebService.Services
{
    public class PuzzleService : IPuzzleService
    {
        private readonly Dictionary<string,List<Puzzle>> _puzzles;
        private readonly Random _random;

        public PuzzleService()
        {
            _puzzles = new Dictionary<string, List<Puzzle>>
            {
                {
                    "en", new List<Puzzle>
                    {
                        new Puzzle {Question = "How many days are a week?", Answer = 7},
                        new Puzzle {Question = "How many million people live in Austria?", Answer = 7},
                        new Puzzle {Question = "how big is the eiffel tower in  meters?", Answer = 300},
                        new Puzzle {Question = "how much tons does a blue whale weigh?", Answer = 17}
                    }
                },
                {
                    "de", new List<Puzzle>
                    {
                        new Puzzle {Question = "Wie viele Tage hat eine Woche?", Answer = 7},
                        new Puzzle {Question = "Wieviele million Leute wohnen in Österreich?", Answer = 7},
                        new Puzzle {Question = "Wie hoch ist der Eiffelturm in Meter?", Answer = 300},
                        new Puzzle {Question = "Wie viele Tonnen wiegt ein Blauwal?", Answer = 17}
                    }
                }
            };
            _random = new Random();
        }

        public Puzzle GetRandomPuzzle(string language)
        {
            return _puzzles[language][_random.Next(_puzzles[language].Count)];
        }

        public IEnumerable<Puzzle> Puzzles(string language) => _puzzles[language];
    }
}