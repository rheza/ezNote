﻿using System;
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
using Microsoft.Phone.Shell;

namespace Zub_App
{
    public partial class CategoryPage : PhoneApplicationPage
    {

        private Category categories;

        int categoryListID = -1;


        int noOfItems;

        public CategoryPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;

            deleteButton = ApplicationBar.MenuItems[1] as ApplicationBarMenuItem;



        }

        private void addNotes(object sender, EventArgs e)
        {
            // Navigate to the new page
            NavigationService.Navigate(new Uri("/AddPage.xaml?catID=" + categoryListID + "&catName=" + this.PageTitle.Text, UriKind.Relative));

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
        }

        private void appbar_backtoMainScreen_Click(object sender, EventArgs e)
        {
            // Navigate to the new page
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));

            // Reset selected index to -1 (no selection)
            MainListBox.SelectedIndex = -1;
        }

        private void appbar_deleteCategory_Click(object sender, EventArgs e)
        {
            categories.Delete();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.LayoutRoot.Background = null;
            this.ReadFromIsolatedStorage("userBackground.jpg");



            PopulateNotes();

            setCategoryName();

            if (noOfItems != 0)
                deleteButton.IsEnabled = false;
            else
                deleteButton.IsEnabled = true;

        }

        private void setCategoryName()
        {

            categories = SterlingService.Current.Database.Query<Category, int>()
                    .Where(delegate(TableKey<Category, int> key) { return key.Key == categoryListID; })
                    .First<TableKey<Category, int>>()
                    .LazyValue.Value;

            this.PageTitle.Text = categories.categoryName;

        }
        private void PopulateNotes()
        {


            int.TryParse(NavigationContext.QueryString["catID"], out categoryListID);

            Debug.WriteLine("CategoryID at category page =  " + categoryListID);
            Debug.Assert(categoryListID != -1, "TaskId should not be null");



            string orderValue;

            if (IsolatedStorageSettings.ApplicationSettings.Contains("orderByName"))
            {
                orderValue = IsolatedStorageSettings.ApplicationSettings["orderByName"].ToString();

                Debug.WriteLine("orderVAlue = " + orderValue);

                if (orderValue == "True")
                {
                    Debug.WriteLine("Order by Namechecked");
                    var noteList = (from k in SterlingService.Current.Database.Query<Notes, int, int>("CategoryID")
                                    where k.Index == categoryListID
                                    orderby k.LazyValue.Value.noteName
                                    select k.LazyValue.Value);

                    this.MainListBox.ItemsSource = noteList;
                }
                else
                {
                    Debug.WriteLine("Order by Datechecked");
                    var noteList = (from k in SterlingService.Current.Database.Query<Notes, int, int>("CategoryID")
                                    where k.Index == categoryListID
                                    orderby k.LazyValue.Value.createDate
                                    select k.LazyValue.Value);

                    this.MainListBox.ItemsSource = noteList;
                }

            }
            else
            {
                Debug.WriteLine("Order by Datechecked");
                var noteList = (from k in SterlingService.Current.Database.Query<Notes, int, int>("CategoryID")
                                where k.Index == categoryListID
                                orderby k.LazyValue.Value.createDate
                                select k.LazyValue.Value);

                this.MainListBox.ItemsSource = noteList;
            }
      


            CountThis myCount = new CountThis();

            string countNo;

            countNo = myCount.countItem(categoryListID).ToString();

            noOfItems = int.Parse(countNo);


        }

        private void MainListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            if (MainListBox.SelectedItem != null)
            {


                var data = MainListBox.SelectedItem as Notes;

                NavigationService.Navigate(new Uri("/DetailsPage.xaml?noteID=" + data.Id, UriKind.Relative));


            }


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