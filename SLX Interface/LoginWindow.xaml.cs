using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace SLX_Interface
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void loginWindowOK_Click(object sender, RoutedEventArgs e)
        {
            bool loginSuccess = false;

            loginProgressBar.Visibility = Visibility.Visible;

            //Throw the login details at the LoginCredentials class
            NetworkCredential inputCred = new NetworkCredential(loginWindowUName.Text, loginWindowPWord.Password);

            LoginCredentials.userCred = inputCred;
                        
            try
            {
                //Using XmlReader to grab UserOptions from SLX
                XmlReader resultsReader = null;

                XmlUrlResolver resultsResolver = new XmlUrlResolver();
                resultsResolver.Credentials = LoginCredentials.userCred;

                XmlReaderSettings resultsReaderSettings = new XmlReaderSettings();
                resultsReaderSettings.XmlResolver = resultsResolver;
                resultsReaderSettings.Async = true;

                using (resultsReader = XmlReader.Create("https://crm.crmcloud.infor.com:443/sdata/slx/system/-/", resultsReaderSettings))
                {
                    while (await resultsReader.ReadAsync())
                    {

                    }
                }

                loginSuccess = true;
            }
            catch (Exception error)
            {
                MessageBox.Show("Login failed with error: \n" + error.Message);
            }

            if (loginSuccess == true)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                Close();
            }

            loginProgressBar.Visibility = Visibility.Hidden;
        }

        private async void loginWindowPWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                bool loginSuccess = false;

                loginProgressBar.Visibility = Visibility.Visible;

                //Throw the login details at the LoginCredentials class
                NetworkCredential inputCred = new NetworkCredential(loginWindowUName.Text, loginWindowPWord.Password);

                LoginCredentials.userCred = inputCred;

                try
                {
                    //Using XmlReader to grab UserOptions from SLX
                    XmlReader resultsReader = null;

                    XmlUrlResolver resultsResolver = new XmlUrlResolver();
                    resultsResolver.Credentials = LoginCredentials.userCred;

                    XmlReaderSettings resultsReaderSettings = new XmlReaderSettings();
                    resultsReaderSettings.XmlResolver = resultsResolver;
                    resultsReaderSettings.Async = true;

                    using (resultsReader = XmlReader.Create("https://crm.crmcloud.infor.com:443/sdata/slx/system/-/", resultsReaderSettings))
                    {
                        while (await resultsReader.ReadAsync())
                        {

                        }
                    }

                    loginSuccess = true;
                }
                catch (Exception error)
                {
                    MessageBox.Show("Login failed with error: \n" + error.Message);
                }

                if (loginSuccess == true)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();

                    Close();
                }

                loginProgressBar.Visibility = Visibility.Hidden;
            }
        }
    }
}
