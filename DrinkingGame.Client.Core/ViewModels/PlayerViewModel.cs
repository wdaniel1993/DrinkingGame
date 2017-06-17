using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace DrinkingGame.Client.Core.ViewModels
{
    public class PlayerViewModel : ReactiveObject
    {
        private int _score;
        private int _answer;
        private string _name = String.Empty;
        private bool _shouldDrink;

        public int Score {
            get => _score;
            set => this.RaiseAndSetIfChanged(ref _score, value);
        }

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public int Answer
        {
            get => _answer;
            set => this.RaiseAndSetIfChanged(ref _answer, value);
        }

        public bool ShouldDrink
        {
            get => _shouldDrink;
            set => this.RaiseAndSetIfChanged(ref _shouldDrink, value);
        }
    }
}
