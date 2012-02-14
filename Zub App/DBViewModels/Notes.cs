using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Tasks;


namespace Zub_App
{
    public class Notes : BaseModel
    {

        private int _noteid;



        public int noteID
        {
            get { return this._noteid; }
            set
            {
                if (value != this._noteid)
                {
                    this._noteid = value;
                    this.OnPropertyChanged("noteID");
                  
                }
            }
        }


        private int _categoryid;

        public int categoryID
        {
            get { return this._categoryid; }
            set
            {
                if (value != this._categoryid)
                {
                    this._categoryid = value;
                    this.OnPropertyChanged("categoryID");

                }
            }
        }

        int _isDue;
        public int isDue
        {
            get { return this._isDue; }

            set
            {
                if (value != this._isDue)
                {
                    this._isDue = value;
                    this.OnPropertyChanged("isDue");
                }
            }
        }


        int _dueCompleted;
        public int dueCompleted
        {
            get { return this._dueCompleted; }

            set
            {
                if (value != this._dueCompleted)
                {
                    this._dueCompleted = value;
                    this.OnPropertyChanged("dueCompleted");
                }
            }
        }


        private DateTime? dueDate;
        public DateTime? DueDate
        {
            get { return this.dueDate; }

            set
            {
                if (value != this.dueDate)
                {
                    this.dueDate = value;
                    this.OnPropertyChanged("DueDate");
                }
            }
        }


        private string _noteName;

        public string noteName
        {
            get
            {
                return this._noteName;
            }
            set
            {
                if (value != this._noteName)
                {
                    this._noteName = value;
                    this.OnPropertyChanged("noteName");
                }
            }
        }
        
        

        private string _noteText;

        public string noteText
        {
            get { return this._noteText; }

            set
            {
                if (value != this._noteText)
                {
                    this._noteText = value;
                    this.OnPropertyChanged("noteText");
                }
            }
        }

       
        private DateTime _createDate;
        public DateTime createDate
        {
            get { return this._createDate; }

            set
            {
                if (value != this._createDate)
                {
                    this._createDate = value;
                    this.OnPropertyChanged("createDate");
                }
            }
        }
        
        public void Delete()
        {
            SterlingService.Current.Database.Delete(this);
            SterlingService.Current.Database.Flush();
        }

        public void Save()
        {
            SterlingService.Current.Database.Save(this);
            SterlingService.Current.Database.Flush();
        }

    }
}
