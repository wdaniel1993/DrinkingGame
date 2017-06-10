using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace DrinkingGame.Client.Core.ViewModels
{
    public class MainPageViewModel : ReactiveObject
    {
        private ReactiveObject _content;

        public MainPageViewModel()
        {
            // Initially show the login view.
            var connectViewModel = new ConnectViewModel();
            _content = connectViewModel;

            connectViewModel.ConnectCommand.Where(x=> x).Subscribe(b =>
            {
                Content = new GameViewModel();
            });
        }

        public ReactiveObject Content
        {
            get => this._content;
            private set => this.RaiseAndSetIfChanged(ref this._content, value);
        }
    }
}
