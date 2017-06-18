using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DrinkingGame.Client.Core.ViewModels;
using ReactiveUI;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DrinkingGame.Raspberry.Views
{
    public sealed partial class PlayerView : IViewFor<PlayerViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(PlayerViewModel),
                typeof(PlayerView),
                new PropertyMetadata(null));

        public PlayerView()
        {
            InitializeComponent();

            this.OneWayBind(ViewModel, vm => vm.Name, view => view.PlayerName.Text);
            this.OneWayBind(ViewModel, vm => vm.Score, view => view.Score.Text);
            this.OneWayBind(ViewModel, vm => vm.ShouldDrink, view => view.Loser.IsChecked);
            this.OneWayBind(ViewModel, vm => vm.Answer, view => view.Answer.Text);
            this.OneWayBind(ViewModel, vm => vm.Answer, view => view.AnswerPanel.Visibility, x => x == default(int) ? Visibility.Collapsed : Visibility.Visible);
            this.OneWayBind(ViewModel, vm => vm.IsDrinking, view => view.PlayerName.FontStyle,
                x => x ? FontStyle.Italic : FontStyle.Normal);
        }

        public PlayerViewModel ViewModel
        {
            get => (PlayerViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (PlayerViewModel)value;
        }
    }
}
