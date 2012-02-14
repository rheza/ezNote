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

namespace Zub_App
{
    public partial class MainPage : PhoneApplicationPage
    {

        
       
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
            Loaded += MainPage_Loaded;

            

        }
        

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.LayoutRoot.Background = null;
            this.ReadFromIsolatedStorage("userBackground.jpg");

            
                PopulateCategories();
               
                
        }

        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            CountThis noNotes = new CountThis();

            string noNotesString;

            noNotesString = noNotes.countAllItem().ToString();

            this.AllNo.Text = noNotesString;

            string noDueString;

            noDueString = noNotes.countOverItem().ToString();

            this.DueNo.Text = noDueString;
        }


        


        private void PopulateCategories()
        {
            
            this.MainListBox.ItemsSource = from k in SterlingService.Current.Database.Query<Category, int>()
                                           orderby k.LazyValue.Value.categoryName
                                           select k.LazyValue.Value;

            foreach (Category o in MainListBox.Items)
            {
                CountThis myCount = new CountThis();

                string countNo;

                countNo = myCount.countItem(o.Id).ToString();

                o.noItems = int.Parse(countNo);

            }
         }


        


        // Handle selection changed on ListBox
        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

           
            
            if (MainListBox.SelectedItem != null)
            {

                var catData = MainListBox.SelectedItem as Category;
                
                NavigationService.Navigate(new Uri("/CategoryPage.xaml?catID=" + catData.Id, UriKind.Relative));
               

            }
          

             MainListBox.SelectedIndex = -1;
            
        }

        //go to all notes
        private void addCategory(object sender, EventArgs e)
        {
            // Navigate to the new page
            NavigationService.Navigate(new Uri("/AddCategory.xaml", UriKind.Relative));

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
        }

        //add Category
        private void all_Clicked(object sender, EventArgs e)
        {
            // Navigate to the new page
            NavigationService.Navigate(new Uri("/AllNotes.xaml", UriKind.Relative));

        }

        private void due_Clicked(object sender, EventArgs e)
        {
            // Navigate to the new page
            NavigationService.Navigate(new Uri("/AllDue.xaml", UriKind.Relative));

        }

        private void addNotes(object sender, EventArgs e)
        {
            // Navigate to the new page
            NavigationService.Navigate(new Uri("/ChooseCategory.xaml", UriKind.Relative));

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
        }

        private void appbar_Setting_Click(object sender, EventArgs e)
        {
            // Navigate to the new page
            NavigationService.Navigate(new Uri("/SettingPage.xaml", UriKind.Relative));
           
        }

        private void appbar_ezNote_Click(object sender, EventArgs e)
        {
            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DownloadNote.xaml", UriKind.Relative));

        }

        private void appbar_button2_Click(object sender, EventArgs e)
        {
            // Navigate to the new page
         

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
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