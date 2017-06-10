using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class ConnectView : IViewFor<ConnectViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                "ViewModel",
                typeof(ConnectViewModel),
                typeof(ConnectView),
                new PropertyMetadata(null));

        public ConnectView()
        {
            InitializeComponent();

            this.WhenAnyObservable(x => x.ViewModel.ConnectCommand.CanExecute).BindTo(this, x => x.OkButton.IsEnabled);

            this.Bind(ViewModel, x => x.GameId, view => view.GameIdField.Text);

            this.BindCommand(ViewModel, x => x.ConnectCommand, x => x.OkButton);
        }

        public ConnectViewModel ViewModel
        {
            get => (ConnectViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ConnectViewModel)value;
        }
    }
}
