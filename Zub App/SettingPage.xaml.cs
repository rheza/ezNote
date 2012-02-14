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


namespace Zub_App
{
    public partial class SettingPage : PhoneApplicationPage
    {
        PhotoChooserTask photoChooserTask;

        public SettingPage()
        {

            InitializeComponent();


            this.ReadFromIsolatedStorage("userBackground.jpg");

            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);

        }
        


        private void checkUploadtoWeb_Checked(object sender, RoutedEventArgs e)
        {
           
        }

        private void orderByDate_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void orderByName_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void backgroundChooser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                photoChooserTask.Show();
            }
            catch (System.InvalidOperationException ex)
            {
                // Catch the exception, but no handling is necessary.
            }
        }

        void photoChooserTask_Completed(object sender, PhotoResult e)
        {

            StringBuilder sb = new StringBuilder();
            if (e.TaskResult == TaskResult.OK)
            {

            
                SaveToIsolatedStorage(e.ChosenPhoto, "userBackground.jpg");
                this.ReadFromIsolatedStorage("userBackground.jpg");
                


              

            }
        }

       public void SaveToIsolatedStorage(Stream imageStream, string fileName)
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (myIsolatedStorage.FileExists(fileName))
                {
                    myIsolatedStorage.DeleteFile(fileName);
                }

                IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(fileName);
                BitmapImage bitmap = new BitmapImage();
                bitmap.SetSource(imageStream);
                
                Image beforeCrop = new Image();
                Image afterCrop = new Image();

                if (bitmap.PixelWidth > bitmap.PixelHeight)
                {
                    afterCrop.Width = 800;
                    afterCrop.Height = 480;
                }
                else
                {
                    afterCrop.Width = 480;
                    afterCrop.Height = 800;
                }
                

                beforeCrop.Source = bitmap;

                WriteableBitmap wb = new WriteableBitmap((int)afterCrop.Width, (int)afterCrop.Height);

                Transform t = new TranslateTransform();
                Point p = new Point((int)afterCrop.Width, (int)afterCrop.Height);
                t.Transform(p);
                wb.Render(beforeCrop, t);
                wb.Invalidate();
                afterCrop.Source = wb;

                


                wb.SaveJpeg(fileStream, (int)afterCrop.Width, (int)afterCrop.Height, 0, 100);

               
              

                fileStream.Close();
            }
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

       private void radioButton1_Checked(object sender, RoutedEventArgs e)
       {

       }
 
        
    }
}