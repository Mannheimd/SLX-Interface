using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace SLX_Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Background workers so we can run searches and stuff without freezing the UI
        private BackgroundWorker accountWorker = new BackgroundWorker();
        private BackgroundWorker ticketWorker = new BackgroundWorker();

        //Build the path to %appdata%\SLX Interface\
        public static string appDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SLX Interface\";

        //===========================//
        //=== MAIN WINDOW IS HERE ===//
        //===========================//
        public MainWindow()
        {
            InitializeComponent();

            getFieldLists();

            accountSearchResultsList.ItemsSource = CachedData.accounts;

            //Account search worker
            accountWorker.DoWork += new DoWorkEventHandler(accountWorker_DoWork);
            accountWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(accountWorker_RunWorkerCompleted);

            //Ticket search worker
            ticketWorker.DoWork += new DoWorkEventHandler(accountWorker_DoWork);
            ticketWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(accountWorker_RunWorkerCompleted);
        }

        //Dev tool for creating XML formatted .txt files from a list of fields stored in .txt format - resulting XML code used for FieldList stuff
        private void createNewXmlList()
        {
            StreamReader file = new StreamReader(@"G:\Users\Chris\Desktop\Input.txt");

            int count = 0;
            while (file.ReadLine() != null)
            {
                count++;
            }
            file.Close();

            StreamReader input = new StreamReader(@"G:\Users\Chris\Desktop\Input.txt");
            for (int i = 0; i < count; i++)
            {
                string line = input.ReadLine();

                using (StreamWriter output = File.AppendText(@"G:\Users\Chris\Desktop\Output.txt"))
                {
                    output.WriteLine("<Account>");
                    output.WriteLine("  <FriendlyName>" + line + "</FriendlyName>");
                    output.WriteLine("  <TrueName>" + line + "</TrueName>");
                    output.WriteLine("  <Display>false</Display>");
                    output.WriteLine("  <Search>false</Search>");
                    output.WriteLine("</Account>");
                }
            }
            MessageBox.Show("Done");
        }

        //Trigger the window to edit what fields the user in the list and search drop-down
        private void editFieldVisibility_Click(object sender, RoutedEventArgs e)
        {

        }

        //Populating the dropdown boxes with user preferences stored in XML
        private void getFieldLists()
        {
            //Getting account fields and caching the data
            XmlDocument fieldXml = new XmlDocument();

            List<SearchableField> accountFieldList = new List<SearchableField>();

            if (File.Exists(appDataFolderPath + "FieldData.xml"))
            {
                fieldXml.Load(appDataFolderPath + "FieldData.xml");
            }
            else
            {
                try
                {
                    if (!Directory.Exists(appDataFolderPath))
                    {
                        Directory.CreateDirectory(appDataFolderPath);
                    }
                    File.Copy("FieldData.xml", appDataFolderPath + "FieldData.xml");

                    fieldXml.Load(appDataFolderPath + "FieldData.xml");
                }
                catch(Exception error)
                {
                    MessageBox.Show("Failed to create FieldData.xml in location '" + appDataFolderPath + "'. \n \n" + 
                        "Bad things happen if the application runs without this. Application will terminate. \n \n" +
                        "Error returned: \n" +
                        error.Message);

                    Application.Current.Shutdown();
                }
            }

            XmlNodeList nodeList = fieldXml.SelectNodes("Fields/Account");

            foreach (XmlNode node in nodeList)
            {
                SearchableField field = new SearchableField();

                XmlNode friendlyName = node.SelectSingleNode("FriendlyName");
                field.FriendlyName = friendlyName.InnerXml;

                XmlNode trueName = node.SelectSingleNode("TrueName");
                field.TrueName = trueName.InnerXml;

                XmlNode display = node.SelectSingleNode("Display");
                if (display.InnerXml == "true")
                {
                    field.Display = true;
                }
                else
                {
                    field.Display = false;
                }

                XmlNode search = node.SelectSingleNode("Search");
                if (search.InnerXml == "true")
                {
                    field.Search = true;
                }
                else
                {
                    field.Search = false;
                }

                accountFieldList.Add(field);
            }
            CachedSearchFields.accounts = accountFieldList;


            //Updating the UI with fields that are enabled as searchable

            List<ComboBoxItem> searchAccountFields = new List<ComboBoxItem>();

            foreach (SearchableField field in CachedSearchFields.accounts)
            {
                if (field.Search == true)
                {
                    ComboBoxItem accountCBoxItem = new ComboBoxItem();
                    accountCBoxItem.Content = field.FriendlyName;
                    accountCBoxItem.Tag = field.TrueName;

                    searchAccountFields.Add(accountCBoxItem);
                }
            }

            accountSearchFields.ItemsSource = searchAccountFields;

            ////Updating the UI with fields that are set to display

            //List<ComboBoxItem> searchAccountFields = new List<ComboBoxItem>();

            //foreach (SearchableField field in CachedSearchFields.accounts)
            //{
            //    if (field.Display == true)
            //    {
            //        ComboBoxItem accountCBoxItem = new ComboBoxItem();
            //        accountCBoxItem.Content = field.FriendlyName;
            //        accountCBoxItem.Tag = field.TrueName;

            //        searchAccountFields.Add(accountCBoxItem);
            //    }
            //}

            //accountSearchFields.ItemsSource = searchAccountFields;
        }

        //Account searching functionalisationalism
        private void accountSearchButton_Click(object sender, RoutedEventArgs e)
        {
            //Update the list view on Main Window using the SearchAccount method, assuming the search box isn't blank
            if (accountSearchBox.Text != "")
            {
                string selectedField = ((ComboBoxItem)accountSearchFields.SelectedItem).Tag.ToString();
                SearchAccount(accountSearchBox.Text, selectedField, accountSearchOperator.Text);
            }
        }

        private void accountSearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //Update the list view on Main Window using the SearchAccount method, assuming the search box isn't blank
                if (accountSearchBox.Text != "")
                {
                    string selectedField = ((ComboBoxItem)accountSearchFields.SelectedItem).Tag.ToString();
                    SearchAccount(accountSearchBox.Text, selectedField, accountSearchOperator.Text);
                }
            }
        }

        private void SearchAccount(string searchTerm, string searchField, string searchOperator)
        {
            //Create the string we'll use for searching
            string urlString = null;

            //Build the search string based on user input, replacing the SubData and Operator accordingly.
            if (searchOperator == "Contains") { urlString = "https://crm.crmcloud.infor.com:443/sdata/slx/dynamic/-/accounts?where=" + searchField + "%20like%20%27%25" + searchTerm + "%25%27%20"; }
            if (searchOperator == "Equals") { urlString = "https://crm.crmcloud.infor.com:443/sdata/slx/dynamic/-/accounts?where=" + searchField + "%20eq%20%27" + searchTerm + "%27%20"; }
            if (searchOperator == "Starts With") { urlString = "https://crm.crmcloud.infor.com:443/sdata/slx/dynamic/-/accounts?where=left(" + searchField + "," + searchTerm.Length + ")%20eq%20%27" + searchTerm + "%27%20"; }

            //Trigger the BackgroundWorker that'll grab the data and cache it, then throw the results at the UI
            if (accountWorker.IsBusy != true)
            {
                //Reset the cache
                //CachedData.accounts = new ObservableCollection<Account>();

                //Run the new search
                accountWorker.RunWorkerAsync(urlString);
                accountSearchProgress.Visibility = Visibility.Visible;
            }
        }

        private void accountWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //Extract the search string
            string urlString = (string)e.Argument;

            //Create the XML reader and then try to pull data from SLX into the document
            XmlReader resultsReader = null;

            try
            {
                //Using XmlReader to grab the search results from SLX
                XmlUrlResolver resultsResolver = new XmlUrlResolver();
                resultsResolver.Credentials = LoginCredentials.userCred;

                XmlReaderSettings resultsReaderSettings = new XmlReaderSettings();
                resultsReaderSettings.XmlResolver = resultsResolver;
                resultsReaderSettings.Async = true;

                resultsReader = XmlReader.Create(urlString, resultsReaderSettings);
            }
            //Woops, something fucked up. Better give the user some random error they have no idea what to do with.
            catch (Exception error)
            {
                MessageBox.Show("Failed to get information from SalesLogix. \n \n" +
                    "Error returned: \n" + error.Message + "\n \n" +
                    "String used in search: \n" + urlString);
            }

            Application.Current.Dispatcher.Invoke(delegate
            {
                CachedData.accounts.Clear();
            });

            using (resultsReader)
            {
                while (resultsReader.ReadToFollowing("slx:Account"))
                {
                    //Setting data from the XML to a new Account object ready to be passed to the list
                    Account account = new Account();

                    resultsReader.MoveToFirstAttribute(); account.SDataKey = resultsReader.Value;
                    resultsReader.ReadToFollowing("slx:AccountName"); account.AccountName = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:AccountNameUpper"); account.AccountNameUpper = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Aka"); account.Aka = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:AlternateKeyPrefix"); account.AlternateKeyPrefix = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:AlternateKeySuffix"); account.AlternateKeySuffix = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:AlternatePhone"); account.AlternatePhone = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:BusinessDescription"); account.BusinessDescription = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:CreateDate"); account.CreateDate = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:CreateUser"); account.CreateUser = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:CreditRating"); account.CreditRating = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:CurrencyCode"); account.CurrencyCode = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Description"); account.Description = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Division"); account.Division = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:DoNotSolicit"); account.DoNotSolicit = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Email"); account.Email = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:EmailType"); account.EmailType = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Employees"); account.Employees = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:ExternalAccountNumber"); account.ExternalAccountNumber = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Fax"); account.Fax = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:ImportSource"); account.ImportSource = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Industry"); account.Industry = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:InternalAccountNumber"); account.InternalAccountNumber = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:LastHistoryBy"); account.LastHistoryBy = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:LastHistoryDate"); account.LastHistoryDate = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:MainPhone"); account.MainPhone = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:ModifyDate"); account.ModifyDate = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:ModifyUser"); account.ModifyUser = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:NationalAccount"); account.NationalAccount = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Notes"); account.Notes = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:NotifyDefects"); account.NotifyDefects = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:NotifyOnClose"); account.NotifyOnClose = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:NotifyOnStatus"); account.NotifyOnStatus = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:OtherPhone1"); account.OtherPhone1 = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:OtherPhone2"); account.OtherPhone2 = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:OtherPhone3"); account.OtherPhone3 = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:ParentAccountNumber"); account.ParentAccountNumber = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:ParentId"); account.ParentId = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Region"); account.Region = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Relationship"); account.Relationship = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Revenue"); account.Revenue = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Score"); account.Score = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:ShortNotes"); account.ShortNotes = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:SicCode"); account.SicCode = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Status"); account.Status = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:SubType"); account.SubType = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:TargetAccount"); account.TargetAccount = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Ticker"); account.Ticker = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:TollFree"); account.TollFree = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:TollFree2"); account.TollFree2 = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Type"); account.Type = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:WebAddress"); account.WebAddress = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:WebAddress2"); account.WebAddress2 = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:WebAddress3"); account.WebAddress3 = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:WebAddress4"); account.WebAddress4 = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:AppId"); account.AppId = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:LastErpSyncUpdate"); account.LastErpSyncUpdate = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:PromotedToAccounting"); account.PromotedToAccounting = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:CreateSource"); account.CreateSource = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:PlanEndDate"); account.PlanEndDate = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:PlanCode"); account.PlanCode = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:BusinessPartner"); account.BusinessPartner = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:VantiveID"); account.VantiveID = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:QualifiedBy"); account.QualifiedBy = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:QualifiedDate"); account.QualifiedDate = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Act_rep_referral"); account.Act_rep_referral = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Num_of_Users"); account.Num_of_Users = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:LeadNotes"); account.LeadNotes = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:TimeFrame"); account.TimeFrame = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Budget"); account.Budget = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Competitor"); account.Competitor = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:TrialStartDate"); account.TrialStartDate = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:LeadType"); account.LeadType = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:AtlasId"); account.AtlasId = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:SlxUsers"); account.SlxUsers = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:UserRange"); account.UserRange = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:PlanstartDate"); account.PlanstartDate = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:IsKeyAccount"); account.IsKeyAccount = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Platform"); account.Platform = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:EloquaId"); account.EloquaId = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:CustomerId"); account.CustomerId = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:ScopusId"); account.ScopusId = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:NsinternalId"); account.NsinternalId = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:NSLastSyncDate"); account.NSLastSyncDate = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:NSLastSyncMessage"); account.NSLastSyncMessage = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:ActVersion"); account.ActVersion = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:SlxVersion"); account.SlxVersion = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:IntlLegacyId1"); account.IntlLegacyId1 = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:IntlLegacyId1Source"); account.IntlLegacyId1Source = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:IntlLegacyId2"); account.IntlLegacyId2 = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:IntlLegacyId2Source"); account.IntlLegacyId2Source = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:LegacySlxLicenseGenKey"); account.LegacySlxLicenseGenKey = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:PreferredLanguage"); account.PreferredLanguage = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:SlxconcurrentUsers"); account.SlxconcurrentUsers = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:SlxnamedUsers"); account.SlxnamedUsers = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Reason"); account.Reason = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:PartnerSubType"); account.PartnerSubType = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:ZuoraID"); account.ZuoraID = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:EmailDomain"); account.EmailDomain = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:IsDesiredEMktg"); account.IsDesiredEMktg = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:LastTouchedInDays"); account.LastTouchedInDays = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:NSUrl"); account.NSUrl = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:ZuoraUrl"); account.ZuoraUrl = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Owner"); account.Owner = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:AccountManager"); account.AccountManager = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:AccountProducts"); account.AccountProducts = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Address"); account.Address = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Addresses"); account.Addresses = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Billings"); account.Billings = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Contacts"); account.Contacts = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Contracts"); account.Contracts = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:CustomerTickets"); account.CustomerTickets = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:DefaultTicketOwner"); account.DefaultTicketOwner = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:DivisionalManager"); account.DivisionalManager = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:ExecutiveSponser"); account.ExecutiveSponser = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:LeadSource"); account.LeadSource = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:OperatingCompany"); account.OperatingCompany = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Opportunities"); account.Opportunities = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Projects"); account.Projects = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:RegionalManager"); account.RegionalManager = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Returns"); account.Returns = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:SalesOrders"); account.SalesOrders = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Sf_Accountactivities"); account.Sf_Accountactivities = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:ShippingAddress"); account.ShippingAddress = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:SLXLicenses"); account.SLXLicenses = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Subsidiary"); account.Subsidiary = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Territory"); account.Territory = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:Tickets"); account.Tickets = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:AccountSummary"); account.AccountSummary = resultsReader.ReadElementContentAsString();
                    resultsReader.ReadToFollowing("slx:ERPTradingAccount"); account.ERPTradingAccount = resultsReader.ReadElementContentAsString();

                    //Passing that ginormous wad of data to the list via the ResultsList Update method
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        CachedData.accounts.Add(account);
                        accountSearchResultsCount.Content = CachedData.accounts.Count;
                    });

                    //Update the listview because... well, because we damn well can
                    //resultsList_Update();
                    //accountSearchResultsListView.ItemsSource = resultsList;
                    //accountSearchResultsCount.Content = CachedData.accounts.Count;
                    //Turns out we can't. Lots of errors. Don't try this.
                    //Actually, this is happening now. Just using xaml DataBinding instead. Something something evil space monkeys...
                }
            }
        }

        private void accountWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            accountSearchProgress.Visibility = Visibility.Hidden;
        }

        //Ticket searching
        private void ticketSearchButton_Click(object sender, RoutedEventArgs e)
            {
                //Update the list view on Main Window using the TicketsSearch method
            
            }

        private void ticketSearchBox_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    //Update the list view on Main Window using the TicketsSearch method
                    SearchAccount(ticketSearchBox.Text, accountSearchFields.Text, accountSearchOperator.Text);
                    accountSearchResultsList.ItemsSource = CachedData.accounts;
                }
        }

        //Opening a window for the selected entity
        private void accountSearchResultsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Account selectedAccount = (Account)accountSearchResultsList.SelectedItem;
        }
    }
}
