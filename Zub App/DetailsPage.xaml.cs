using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Resources;
using Microsoft.Phone;
using System.Text;
using System.Threading;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Net.NetworkInformation;
using System.Collections.ObjectModel;
using Microsoft.Phone.Shell;
using Wintellect.Sterling.Keys;
using Wintellect.Sterling.Database;
using Phone.Controls;

namespace Zub_App
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        private ProgressIndicator progress;

        ApplicationBarIconButton upButton;
        int noteID = -1;

        private Notes note;

        public string link;
        public string linkPostTweet;
        public string linkPostFB;
        int catID = -1;

        private Category setCategory;


        private string dateUpFormat;

        private string categoryName;






        // Constructor
        public DetailsPage()
        {
            InitializeComponent();


            string uploadValue;

            completeButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;

            upButton = ApplicationBar.Buttons[3] as ApplicationBarIconButton;



            if (IsolatedStorageSettings.ApplicationSettings.Contains("checkUploadtoWeb"))
            {
                uploadValue = IsolatedStorageSettings.ApplicationSettings["checkUploadtoWeb"].ToString();



                if (uploadValue == "True")
                {
                    upButton.IsEnabled = true;
                }
                else
                {
                    upButton.IsEnabled = false;
                }
            }

            else
            {
                upButton.IsEnabled = true;
            }
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.LayoutRoot.Background = null;
            this.ReadFromIsolatedStorage("userBackground.jpg");
        }


        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            base.OnNavigatedTo(e);

            InitNote();
            setCategoryName();
            DataContext = note;

        }

        private void InitNote()
        {


            int.TryParse(NavigationContext.QueryString["noteID"], out noteID);


            Debug.Assert(noteID != -1, "TaskId should not be null");




            note = SterlingService.Current.Database.Query<Notes, int>()
                     .Where(delegate(TableKey<Notes, int> key) { return key.Key == noteID; })
                     .First<TableKey<Notes, int>>()
                     .LazyValue.Value;


            catID = note.categoryID;
            Debug.WriteLine(note.DueDate);
            Debug.WriteLine(note.isDue);
            Debug.WriteLine(note.dueCompleted);

            if (note.isDue == 0)
            {
                completeButton.IsEnabled = false;
                // this.DueDatePanel.Visibility = Visibility.Collapsed;
                this.dueDateText.Text = "No Due Date";
            }
            else
            {
                // this.DueDatePanel.Visibility = Visibility.Visible;
                if (note.dueCompleted == 0)
                    this.dueDateText.Text = "Due on " + note.DueDate.ToString();
                else
                    this.dueDateText.Text = "Completed";
            }


            if (note.dueCompleted == 1)
                completeButton.IsEnabled = false;
        }

        private void setCategoryName()
        {
            setCategory = SterlingService.Current.Database.Query<Category, int>()
                                .Where(delegate(TableKey<Category, int> key) { return key.Key == catID; })
                                .First<TableKey<Category, int>>()
                                .LazyValue.Value;

            categoryName = setCategory.categoryName;
        }

        private void showProgress()
        {

            this.progress = new ProgressIndicator();

            progress.ProgressType = ProgressTypes.WaitCursor;

            progress.Show();

        }

        private void killProgress()
        {
            progress.Hide();

        }

        private void appbar_Complete_Click(object sender, EventArgs e)
        {


            note.dueCompleted = 1;
            note.Save();
            this.dueDateText.Text = "Completed";
            completeButton.IsEnabled = false;
        }

        private void appbar_editButton_Click(object sender, EventArgs e)
        {

            NavigationService.Navigate(new Uri("/AddPage.xaml?editNoteID=" + noteID + "&catID=" + catID, UriKind.Relative));

        }

        private void appbar_button1_Click(object sender, EventArgs e)
        {
            note.Delete();

            NavigationService.Navigate(new Uri("/CategoryPage.xaml?catID=" + catID, UriKind.Relative));

        }





        private void appbar_button3_Click(object sender, EventArgs e)
        {



            if (NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.None)
            {
                MessageBoxResult res = MessageBox.Show("Network Connection", "ezNote For Windows Phone 7 requires a network connection to upload note.", MessageBoxButton.OK);


                if (res == MessageBoxResult.OK)
                {

                }
            }
            else
            {

                this.ApplicationBar.IsVisible = false;

                showProgress();

                string url;
                string data;
                url = "http://thisco.de/eznote/";

                Debug.WriteLine("is due = " + note.isDue);

                if (note.isDue == 0)
                {
                    data = "shorten.php?title=" + ListTitle.Text + "&note=" + ContentText.Text + "&categoryname=" + categoryName;
                }
                else
                {

                    DateTime dt = DateTime.Parse(note.DueDate.ToString());

                    dateUpFormat = dt.ToString("yyyy-MM-dd HH:mm:ss");
                    Debug.WriteLine(dt);
                    Debug.WriteLine(dateUpFormat);

                    data = "shorten.php?title=" + ListTitle.Text + "&note=" + ContentText.Text + "&duedate= " + dateUpFormat + "&categoryname=" + categoryName;
                }


                WebClient client = new WebClient();
                StringBuilder parameter = new StringBuilder();
                client.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                client.Encoding = Encoding.UTF8;
                client.UploadStringCompleted += new UploadStringCompletedEventHandler(OnUploadStringCompleted);

                Debug.WriteLine(url + data);

                client.UploadStringAsync(new Uri(url + data), "POST");



            }







        }



        Popup p = new Popup();

        void OnUploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {

            Debug.WriteLine(e.Result);

            string noteurl = e.Result;

            Border border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.White);
            border.BorderThickness = new Thickness(0.0);

            StackPanel panel1 = new StackPanel();
            panel1.Background = new SolidColorBrush(Colors.Black);
            Button button1 = new Button();
            button1.Content = "Close";
            button1.FontSize = 22;
            button1.Margin = new Thickness(10, 220, 10, 5);
            button1.Click += new RoutedEventHandler(button1_Click);
            button1.FontFamily = new FontFamily("PhoneFontFamilySemiBold");
            button1.HorizontalContentAlignment = HorizontalAlignment.Left;
            button1.Padding = new Thickness(10.0);

            Button buttonSMS = new Button();
            buttonSMS.Content = "Send link by SMS";
            buttonSMS.FontSize = 22;
            buttonSMS.HorizontalContentAlignment = HorizontalAlignment.Left;
            buttonSMS.FontFamily = new FontFamily("PhoneFontFamilySemiBold");
            buttonSMS.Margin = new Thickness(10, 3, 10, 5);
            buttonSMS.Click += new RoutedEventHandler(buttonSMS_Click);
            buttonSMS.Padding = new Thickness(10.0);

            Button buttonEmail = new Button();
            buttonEmail.Content = "Send link by Email";
            buttonEmail.FontSize = 22;
            buttonEmail.FontFamily = new FontFamily("PhoneFontFamilySemiBold");
            buttonEmail.HorizontalContentAlignment = HorizontalAlignment.Left;
            buttonEmail.Margin = new Thickness(10, 3, 10, 5);
            buttonEmail.Click += new RoutedEventHandler(buttonEmail_Click);
            buttonEmail.Padding = new Thickness(10.0);

            Button postTweet = new Button();
            postTweet.Content = "Share link to Twitter";
            postTweet.FontSize = 22;
            postTweet.FontFamily = new FontFamily("PhoneFontFamilySemiBold");
            postTweet.HorizontalContentAlignment = HorizontalAlignment.Left;
            postTweet.Margin = new Thickness(10, 3, 10, 5);
            postTweet.Click += new RoutedEventHandler(buttonTweet_Click);
            postTweet.Padding = new Thickness(10.0);

            Button postFacebook = new Button();
            postFacebook.Content = "Share link to Facebook";
            postFacebook.FontSize = 22;
            postFacebook.FontFamily = new FontFamily("PhoneFontFamilySemiBold");
            postFacebook.HorizontalContentAlignment = HorizontalAlignment.Left;
            postFacebook.Margin = new Thickness(10, 3, 10, 5);
            postFacebook.Click += new RoutedEventHandler(buttonFB_Click);
            postFacebook.Padding = new Thickness(10.0);

            TextBlock titleBlock = new TextBlock();
            titleBlock.Text = "Link";
            // titleBlock.FontFamily = new FontFamily("PhoneFontFamilyBold");
            titleBlock.FontSize = 32;
            titleBlock.Foreground = new SolidColorBrush(Colors.White);
            titleBlock.FontWeight = FontWeights.SemiBold;

            titleBlock.Margin = new Thickness(24, 48, 10, 5);

            TextBlock textblock1 = new TextBlock();
            textblock1.Text = noteurl;
            textblock1.FontSize = 24;
            textblock1.FontFamily = new FontFamily("PhoneFontFamilySemiBold");
            textblock1.Margin = new Thickness(24, 24, 0, 15);
            textblock1.Foreground = new SolidColorBrush(Colors.White);
            textblock1.Width = 460;
            textblock1.TextWrapping = TextWrapping.Wrap;

            panel1.Children.Add(titleBlock);
            panel1.Children.Add(textblock1);
            panel1.Children.Add(buttonEmail);
            panel1.Children.Add(buttonSMS);
            panel1.Children.Add(postTweet);
            panel1.Children.Add(postFacebook);
            panel1.Children.Add(button1);
            border.Child = panel1;

            // Set the Child property of Popup to the border 
            // which contains a stackpanel, textblock and button.
            p.Child = border;

            // Set where the popup will show up on the screen.
            p.VerticalOffset = 0;
            p.HorizontalOffset = 0;


            link = noteurl;

            killProgress();

            // Open the popup.
            p.IsOpen = true;

        }

        void button1_Click(object sender, EventArgs e)
        {
            // Close the popup.
            p.IsOpen = false;
            this.ApplicationBar.IsVisible = true;
        }

        void buttonSMS_Click(object sender, EventArgs e)
        {
            p.IsOpen = false;

            SmsComposeTask smsComposeTask = new SmsComposeTask();

            smsComposeTask.Body = link;

            smsComposeTask.Show();


        }
        //check network status


        void buttonEmail_Click(object sender, EventArgs e)
        {
            // Close the popup.
            p.IsOpen = false;
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Body = link;

            emailComposeTask.Show();

        }

        //Post To Twitter

        void buttonTweet_Click(object sender, EventArgs e)
        {
            linkPostTweet = link + "&via=eznote";

            Open(linkPostTweet);
        }

        private const string URL = "http://twitter.com/share?url={0}";

        public static void Open(string linkPostTweet)
        {
            WebBrowserTask t = new WebBrowserTask();
            t.URL = String.Format(URL, HttpUtility.UrlEncode(linkPostTweet));
            t.Show();
        }

        //post to facebook
        void buttonFB_Click(object sender, EventArgs e)
        {

            linkPostFB = link;

            OpenFB(linkPostFB);
        }

        private const string URLFB = "http://facebook.com/sharer.php?u={0}";

        public static void OpenFB(string linkPostFB)
        {

            WebBrowserTask f = new WebBrowserTask();
            f.URL = String.Format(URLFB, HttpUtility.UrlEncode(linkPostFB));
            f.Show();
        }

        public void ReadFromIsolatedStorage(string fileName)
        {


            WriteableBitmap bitmap = new WriteableBitmap(480, 800);
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!myIsolatedStorage.FileExists(fileName))
                {
                    Uri uri = new Uri("Images/bg.jpg", UriKind.Relative);
                    BitmapImage imgSource = new BitmapImage(uri);

                    ImageBrush BackgroundFanArt = new ImageBrush();

                    BackgroundFanArt.ImageSource = imgSource;

                    this.LayoutRoot.Background = BackgroundFanArt;
                }
                else
                {
                    using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                    {


                        BitmapImage backgroundImage = new BitmapImage();

                        backgroundImage.SetSource(fileStream);

                        ImageBrush BackgroundFanArt = new ImageBrush();

                        BackgroundFanArt.ImageSource = backgroundImage;
                        BackgroundFanArt.Stretch = Stretch.UniformToFill;




                        this.LayoutRoot.Background = BackgroundFanArt;



                    }
                }
            }
        }

    }
}