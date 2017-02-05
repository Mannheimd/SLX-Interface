using System.Windows;

namespace SLX_Interface
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Application startup
        public void Application_Startup(object sender, StartupEventArgs e)
        {
            //Create and spawn login window
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            //MainWindow mainWindow = new MainWindow();
            //mainWindow.Show();
        }

        // Unhandled exception handler
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message, "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
