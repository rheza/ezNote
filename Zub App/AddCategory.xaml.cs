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
using Wintellect.Sterling.Keys;
using Wintellect.Sterling.Indexes;
using Wintellect.Sterling.Database;
using Wintellect.Sterling;

namespace Zub_App
{
    public partial class AddCategory : PhoneApplicationPage
    {

        public int Id
        {
            get;
            set;
        }

        private Category newCategory;
        
        public AddCategory()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.LayoutRoot.Background = null;
            this.ReadFromIsolatedStorage("userBackground.jpg");
            this.DataContext = newCategory;
            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            InitCategory();
            this.DataContext = newCategory;
        }


        private void InitCategory()
        {
            if (newCategory != null) return;




            newCategory = new Category();

            int newID = newCategory.Id;
                Debug.WriteLine("category = " + newID);
                Debug.WriteLine("category ID = " + newCategory.categoryID);
                Debug.Assert(newCategory != null, "Task should not be null");

                
               // newCategory.categoryID = newID;     
                
        }

        private void addCategory(object sender, EventArgs e)
        {


            if (catTitle.Text == "" )
            {
                MessageBoxResult res = MessageBox.Show(" Please fill in Title and Note first", "You are saving empty note", MessageBoxButton.OK);

                if (res == MessageBoxResult.OK)
                {
                    Debug.WriteLine("Please fill title and note first");
                }

            }
            else
            {

                newCategory.categoryName = catTitle.Text;
                
                newCategory.Save();
                

                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }


        private void cancel(object sender, EventArgs e)
        {
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

        public class DummyProjectViewModel
        {
            public string Name
            {
                get;
                set;
            }

            public int Id
            {
                get;
                set;
            }
        }

    }
}