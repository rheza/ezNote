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
using System.Windows.Controls.Primitives;
using Wintellect.Sterling.Indexes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Wintellect.Sterling.Database;
using Wintellect.Sterling.Keys;
using System.Xml;
using System.Xml.Linq;
using Phone.Controls;

namespace Zub_App
{
    public partial class DownloadNote : PhoneApplicationPage
    {
        private Category newCategories;
        private Notes notes;

        private string notetitle;
        private string notetext;
        private DateTime noteduedate;
        private string categoryname;
        private string status;

        private string dummyduedate;
        private bool duedate;

        private ProgressIndicator progress;

        

        public DownloadNote()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;

            
        }
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.LayoutRoot.Background = null;
            this.ReadFromIsolatedStorage("userBackground.jpg");

          
        }
        //download note
        private void DoWebClient()
        {
            if (ezNoteID.Text == "")
            {

                MessageBoxResult res = MessageBox.Show(" Please fill in ezNote ID first", "ezNote ID Not found", MessageBoxButton.OK);

                if (res == MessageBoxResult.OK)
                {
                    
                }

                
            }
            else
            {
                showProgress();

                WebClient webClient = new WebClient();
                webClient.DownloadStringAsync(new Uri("http://www.thisco.de/eznote/decoderxml.php?decode=" + ezNoteID.Text));
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_OpenReadCompleted);

                
                Debug.WriteLine("http://www.thisco.de/eznote/decoderxml.php?decode=" + ezNoteID.Text);
                Debug.WriteLine("Do Web Client()");
            }

        }

        void webClient_OpenReadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Debug.WriteLine(e.Result);
            /*
            Stream str = e.Result;
            Debug.WriteLine(str);
            
            using (var reader = new StreamReader(e.Result))
            { */
                XElement xelement = XElement.Parse(e.Result.ToString());
                IEnumerable<XElement> notes = xelement.Elements();

                foreach (var note in notes)
                {
                    notetitle = (string)note.Element("title");
                    notetext = (string)note.Element("notetext");
                    dummyduedate = (string)note.Element("duedate");
                    categoryname = (string)note.Element("categoryname");
                    status = (string)note.Element("status");
                }


                

                if (status=="Found")
                {
                    Debug.WriteLine(DateTime.Today.Date);
                    DateTime value;

                    if (!DateTime.TryParse(dummyduedate, out value))
                    {
                        duedate = false;
                    }
                    else
                    {
                        duedate = true;
                        noteduedate = Convert.ToDateTime(dummyduedate);

                        Debug.WriteLine("note due date:"+ noteduedate);
                    }
                    Debug.WriteLine("Your Note");
                    Debug.WriteLine(notetitle);
                    Debug.WriteLine(notetext);
                    Debug.WriteLine(noteduedate);
                    Debug.WriteLine(categoryname);

                    checkCategory(categoryname);

                }
                else
                {


                    

                    Debug.WriteLine("status" + status);

                    MessageBoxResult res = MessageBox.Show("ezNote note not found, please enter the correct ezNote ID", "Not Found", MessageBoxButton.OK);

                    if (res == MessageBoxResult.OK)
                    {
                        killProgress();
                        
                      //  NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                        
                    }

                    status = "Not Found";
                }
                
                
                
         //  }
        }


        private void checkCategory(string catname)
        {
           

            var checkcategories = (from k in SterlingService.Current.Database.Query<Category, int>()
                                   where k.LazyValue.Value.categoryName == catname
                                   select k.LazyValue.Value).FirstOrDefault(); 

                if (checkcategories == null)
                {

                    MessageBoxResult res = MessageBox.Show("Would You like to create category named \"" + catname + "\" ?", "Category not found", MessageBoxButton.OKCancel);

                    if (res == MessageBoxResult.OK)
                    {
                        killProgress();
                        newCategories = new Category();
                        newCategories.categoryName = catname;
                        newCategories.Save();
                        //  NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                        checkCategory(catname);

                    }
                    else
                    {
                        killProgress();

                    }
                }

                else
                {
                    Debug.WriteLine(checkcategories.Id);
                    Debug.WriteLine("category found");

                    notes = new Notes();

                    notes.categoryID = checkcategories.Id;
                    notes.noteName = notetitle;
                    notes.noteText = notetext;
                    notes.createDate = DateTime.Now;

                    if (!duedate)
                    {

                    }
                    else
                    {
                        notes.DueDate = noteduedate;
                        notes.isDue = 1;
                    }
                    Debug.WriteLine("notes.DueDate:"+ notes.DueDate);
                    notes.Save();

                    killProgress();

                    NavigationService.Navigate(new Uri("/CategoryPage.xaml?catID=" + notes.categoryID, UriKind.Relative));



                }
            
            
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


        private void process_ezNoteOnline_Click(object sender, EventArgs e)
        {
            
            
            DoWebClient();

           
        }
        private void cancel_Click(object sender, EventArgs e)
        {
            // Navigate to the new page
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));


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