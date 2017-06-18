using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DrinkingGame.Client.Core.Hubs;
using DrinkingGame.Client.Core.IoT;
using DrinkingGame.Client.Core.ViewModels;
using DrinkingGame.Raspberry.Views;
using Microsoft.AspNet.SignalR.Client;
using ReactiveUI;
using Splat;
using DependencyResolverMixins = Splat.DependencyResolverMixins;

namespace DrinkingGame.Raspberry
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            RegisterDependencies();

            InitializeComponent();
            Suspending += OnSuspending;
        }

        private void RegisterDependencies()
        {
            Locator.CurrentMutable.RegisterConstant(new HubConnection(@"https://poldiapi.azurewebsites.net/"),typeof(HubConnection));
            //Locator.CurrentMutable.RegisterConstant(new HubConnection(@"http://localhost:62562/"), typeof(HubConnection));
            Locator.CurrentMutable.RegisterConstant(new DrinkingGameHubProxy(Locator.CurrentMutable.GetService<HubConnection>(), "DrinkingGameHub"), typeof(DrinkingGameHubProxy));
            if (Package.Current.Id.Architecture == ProcessorArchitecture.Arm)
            {
                Locator.CurrentMutable.RegisterConstant(new MotionSensorService(26), typeof(IMotionSensorService));
            }
            

            Locator.CurrentMutable.Register(() => new ConnectView(), typeof(IViewFor<ConnectViewModel>));
            Locator.CurrentMutable.Register(() => new GameView(), typeof(IViewFor<GameViewModel>));
            Locator.CurrentMutable.Register(() => new PlayerView(), typeof(IViewFor<PlayerViewModel>));
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
