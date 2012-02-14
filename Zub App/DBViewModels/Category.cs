using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.IO.IsolatedStorage;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;
using Wintellect.Sterling;
namespace Zub_App
{

    public class Category : BaseModel
    {
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


        private string _categoryName;

        public string categoryName
        {
            get
            {
                return this._categoryName;
            }
            set
            {
                if (value != this._categoryName)
                {
                    this._categoryName = value;
                    this.OnPropertyChanged("categoryName");
                }
            }
        }

        private int _noItems;

        public int noItems
        {
            get { return this._noItems; }
            set
            {
                if (value != this._noItems)
                {
                    this._noItems = value;
                    this.OnPropertyChanged("noItems");

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

