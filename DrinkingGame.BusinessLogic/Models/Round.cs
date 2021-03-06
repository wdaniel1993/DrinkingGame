﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using DrinkingGame.BusinessLogic.States;

namespace DrinkingGame.BusinessLogic.Models
{
    public class Round
    {
        private readonly Subject<Guess> _guessAdded = new Subject<Guess>();
        private readonly Subject<IEnumerable<Player>> _roundCompleted = new Subject<IEnumerable<Player>>();
        private readonly Subject<Player> _drinkTaken = new Subject<Player>();
        private readonly List<Guess> _guesses = new List<Guess>();
        private readonly List<Player> _losersDrank = new List<Player>();
        private bool _isCompleted;
        private bool _isLastRound;
        private bool _guessesCompleted;
        private bool _drinksCompleted;

        public bool IsCompleted => _isCompleted;
        public bool GuessesCompleted => _guessesCompleted;
        public bool IsLastRound => _isLastRound;
        public bool DrinksCompleted => _drinksCompleted;

        public Puzzle Puzzle { get; set; }
        public IEnumerable<Guess> Guesses => _guesses.AsReadOnly();
        public IEnumerable<Player> LosersDrank => _losersDrank.AsReadOnly();
        public IObservable<Guess> GuessesAdded => _guessAdded.AsObservable();
        public IObservable<IEnumerable<Player>> RoundCompleted => _roundCompleted.AsObservable();
        public IObservable<Player> DrinkTaken => _drinkTaken.AsObservable();
        public Game Game { get; set; }

        public IEnumerable<Player> Losers => Guesses.MaxBy(x => Math.Abs(x.Estimate - Puzzle.Answer)).Select(x => x.Player);

        public void AddGuess(Guess guess)
        {
            if (!_guesses.Exists(x => x.Player == guess.Player))
            {
                _guesses.Add(guess);
                _guessAdded.OnNext(guess);

                if (_guesses.Count == Game.Players.Count())
                {
                    _guessAdded.OnCompleted();
                    _guessesCompleted = true;
                }
            }
        }

        public void PlayerDrank(Player player)
        {
            if (Losers.Any(x => x.Name == player.Name) && !_losersDrank.Contains(player))
            {
                _losersDrank.Add(player);
                _drinkTaken.OnNext(player);
            }
            if (_losersDrank.Count == Losers.Count())
            {
                _drinkTaken.OnCompleted();
                _drinksCompleted = true;
            }
        }

        public void CompleteRound(bool isLastRound)
        {
            Game.Players.Where(x => Losers.Select(l => l.Name).ToList().Contains(x.Name)).ForEach(x => x.Score++);
            _isCompleted = true;
            _isLastRound = isLastRound;
            _roundCompleted.OnNext(Losers);
        }

        public void IgnoreDrinks()
        {
            _drinkTaken.OnCompleted();
            _drinksCompleted = true;
        }
    }
}
