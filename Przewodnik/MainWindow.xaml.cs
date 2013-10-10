using System;
using System.Windows;

using Przewodnik.Utilities;
using Przewodnik.Controllers;
using System.Windows.Input;

namespace Przewodnik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int MinimumScreenWidth = 1920;
        private const int MinimumScreenHeight = 1080;

        /// <summary>
        /// Mouse movement detector.
        /// </summary>
        private readonly MouseMovementDetector mouseMovementDetector;

        /// <summary>
        /// Controller used as ViewModel for this window.
        /// </summary>
        private readonly KinectController kinectController;

        public MainWindow(KinectController kinectController)
        {
            InitializeComponent();

            if (kinectController == null)
            {
                throw new ArgumentNullException("controller", Properties.Resources.KinectControllerInvalid);
            }

            this.kinectController = kinectController;

            this.mouseMovementDetector = new MouseMovementDetector(this);
            this.mouseMovementDetector.IsMovingChanged += this.OnIsMouseMovingChanged;
            this.mouseMovementDetector.Start();
        }

        /// <summary>
        /// Handles Window.Loaded event, and prompts user if screen resolution does not meet
        /// minimal requirements.
        /// </summary>
        /// <param name="sender">
        /// Object that sent the event.
        /// </param>
        /// <param name="e">
        /// Event arguments.
        /// </param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            // get the main screen size
            double height = SystemParameters.PrimaryScreenHeight;
            double width = SystemParameters.PrimaryScreenWidth;

            // if the main screen is less than 1920 x 1080 then warn the user it is not the optimal experience 
            if ((width < MinimumScreenWidth) || (height < MinimumScreenHeight))
            {
                MessageBoxResult continueResult = MessageBox.Show(Properties.Resources.SmallerScreenResolutionMessage, Properties.Resources.SmallerScreenResolutionTitle, MessageBoxButton.YesNo);
                if (continueResult == MessageBoxResult.No)
                {
                    this.Close();
                }
            }
        }

        /// <summary>
        /// Handles MouseMovementDetector.IsMovingChanged event and shows/hides the window bezel,
        /// as appropriate.
        /// </summary>
        /// <param name="sender">
        /// Object that sent the event.
        /// </param>
        /// <param name="e">
        /// Event arguments.
        /// </param>
        private void OnIsMouseMovingChanged(object sender, EventArgs e)
        {
            WindowBezelHelper.UpdateBezel(this, this.mouseMovementDetector.IsMoving);
            this.kinectController.IsInEngagementOverrideMode = this.mouseMovementDetector.IsMoving;
        }

    }
}
