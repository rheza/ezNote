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


namespace Zub_App
{
    public partial class ChooseCategory : PhoneApplicationPage
    {
        public ChooseCategory()
        {
            InitializeComponent();
            PopulateCategories();
            Loaded += MainPage_Loaded;


        }
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.LayoutRoot.Background = null;
            this.ReadFromIsolatedStorage("userBackground.jpg");


        }

        

        private void PopulateCategories()
        {

            this.chooseCategoryListBox.ItemsSource = from k in SterlingService.Current.Database.Query<Category, int>()
                                           select k.LazyValue.Value;
        }

        private void chooseCategoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {




            if (chooseCategoryListBox.SelectedItem != null)
            {
                var catData = chooseCategoryListBox.SelectedItem as Category;

                NavigationService.Navigate(new Uri("/AddPage.xaml?catID=" + catData.Id + "&catName=" + catData.categoryName , UriKind.Relative));

            }


            chooseCategoryListBox.SelectedIndex = -1;

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