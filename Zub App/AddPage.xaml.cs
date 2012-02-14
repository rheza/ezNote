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
using System.Windows.Data;


namespace Zub_App
{
  
    public partial class AddPage : PhoneApplicationPage
    {
        private Notes notes;
        private Category categories;
        private DateTime selectedTime;
        private DateTime selectedDate;

        private int catID;
        private int noteDueChecked;

        bool isItEdit;
        private bool isLoaded;
       
        public AddPage()
        {

            InitializeComponent(); 
            
            Debug.WriteLine("is it edit  in public add page="+ isItEdit);

            this.Loaded += this.OnLoaded;
            this.Unloaded += this.OnUnloaded;

            this.dueDateSet.ValueChanged += new EventHandler<DateTimeValueChangedEventArgs>(dueDate_ValueChanged);

            this.dueTimeSet.ValueChanged += new EventHandler<DateTimeValueChangedEventArgs>(dueTime_ValueChanged);

            this.dueStack.Visibility = Visibility.Collapsed;

            
                
            
            
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
		{
            
            if (NavigationContext.QueryString.ContainsKey("catID"))
            {
                int.TryParse(NavigationContext.QueryString["catID"], out catID);

                categories = SterlingService.Current.Database.Query<Category, int>()
                            .Where(delegate(TableKey<Category, int> key) { return key.Key == catID; })
                            .First<TableKey<Category, int>>()
                            .LazyValue.Value;


                catTitle.Text = categories.categoryName;

                notes.categoryID = categories.Id;


            }
            
			this.isLoaded = true;
		}

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.isLoaded = false;

        }
        
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            this.LayoutRoot.Background = null;
            this.ReadFromIsolatedStorage("userBackground.jpg");



           

            if (!NavigationContext.QueryString.ContainsKey("editNoteID"))
            {

                isItEdit = false;

                

            }
            else
            {

                isItEdit = true;


            }

            InitTask();
            InitCategoryList();
            

            this.DataContext = notes;

         
        }

        
     
       

        private void button2_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void setDue_Click(object sender, EventArgs e)
        {
            
            if (setDue.IsChecked == true)
            {

                noteDueChecked = 1;
                this.dueStack.Visibility = Visibility.Visible;

                
                 

                

            }
            else
            {
                
                this.dueStack.Visibility = Visibility.Collapsed;

                DateTime dt1 = DateTime.Now;


                dueDateSet.Value = DateTime.Parse(dt1.ToString("M/dd/yyyy"));

                

                dueTimeSet.Value = DateTime.Parse(dt1.ToString("hh:mm tt"));

            }
        }

        

        private void validateLength(object sender, EventArgs e)
        {
           

            if (textTitle.Text == "" || textNote.Text == "")
            {
                MessageBoxResult res = MessageBox.Show(" Please fill in Title and Note first", "You are saving empty note", MessageBoxButton.OK);

                if (res == MessageBoxResult.OK)
                {
                    Debug.WriteLine("Please fill title and note first"); 
                }
                
            }
            else
            {

                DateTime Date = DateTime.Now;

                String newDueDate = ("") + selectedDate.ToString("M/d/yyyy") + " " + selectedTime.ToString("h:mm:ss tt");

                Debug.WriteLine("New Due Date" + newDueDate);
                notes.DueDate = Convert.ToDateTime(newDueDate);

                if (noteDueChecked == 1)
                {
                    notes.isDue = 1;

                }
                else
                {
                    notes.isDue = 0;
                    notes.DueDate = DateTime.Now;

                }
                
                
                notes.categoryID = catID;
                notes.noteName = textTitle.Text;
                notes.noteText = textNote.Text;
                notes.createDate = Date;
               
                

                
                

                
                

                
              
                notes.Save();

                NavigationService.Navigate(new Uri("/CategoryPage.xaml?catID=" + catID, UriKind.Relative));
            }
        }

        private void dueTime_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
       {
           selectedTime = (DateTime)dueTimeSet.Value;
           Debug.WriteLine("Duetime :" + selectedTime.ToString("h:mm:ss tt"));

        }

        private void dueDate_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
         {
             selectedDate = (DateTime)dueDateSet.Value;
             Debug.WriteLine("DueDate :" + selectedDate.ToString("M/d/yyyy"));
             
        }

        private void InitCategoryList()
        {
            var categorieslist = from k in SterlingService.Current.Database.Query<Category, int>()
                                 select k.LazyValue.Value.categoryName;
         
        }

        private void InitTask()
        {
           // if (notes != null) return;
            Debug.WriteLine("isItEdit : "+isItEdit);
            if (!isItEdit)
            {
             
                notes = new Notes();
                
                
            }
            else
            {
                PageTitle.Text = "Edit ezNote";
                Debug.WriteLine("this is edit on inittask");
                int editNoteID;
                int.TryParse(NavigationContext.QueryString["editNoteID"], out editNoteID);

                Debug.WriteLine("Edit Note ID="+editNoteID);

                notes = SterlingService.Current.Database.Query<Notes, int>()
                        .Where(delegate(TableKey<Notes, int> key) { return key.Key == editNoteID; })
                        .First<TableKey<Notes, int>>()
                        .LazyValue.Value;

                
                textTitle.Text = notes.noteName;
                textNote.Text = notes.noteText;

                Debug.WriteLine("is it due = " + notes.isDue);
                if (notes.isDue == 1)
                {
                    Debug.WriteLine("is due is checked");
                    setDue.IsChecked = true;
                    noteDueChecked = 1;
                    this.dueStack.Visibility = Visibility.Visible;
                }

                DateTime dt2;

                

                

                dt2 = DateTime.Parse(notes.DueDate.ToString());


                Debug.WriteLine(notes.DueDate);

                dueDateSet.Value = DateTime.Parse(dt2.ToString("MM/dd/yyyy"));

                dueTimeSet.Value = DateTime.Parse(dt2.ToString("hh:mm tt"));

            }


                Debug.Assert(notes != null, "Task should not be null");
            
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