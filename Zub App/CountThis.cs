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
using System.Linq;
using System.Diagnostics;

namespace Zub_App
{
    public class CountThis
    {
        public int noOfNote;
        public object countItem(int categoryID)
        {
            
            var AllNoOfItems = (from k in SterlingService.Current.Database.Query<Notes, int>()
                                where k.LazyValue.Value.categoryID == categoryID
                                select k.LazyValue.Value).FirstOrDefault();


            if (AllNoOfItems == null)
            {
               noOfNote = 0;
            }
            else
            {
                var setAllNoOfItems = from k in SterlingService.Current.Database.Query<Notes, int>()
                                         where k.LazyValue.Value.categoryID == categoryID
                                         select k.LazyValue;
                noOfNote = setAllNoOfItems.Count();
            }

            
            return noOfNote;
        }

        public int noOfAllNote;
        public object countAllItem()
        {

            var AllNoOfItems = (from k in SterlingService.Current.Database.Query<Notes, int>()
                                select k.LazyValue.Value).FirstOrDefault();


            if (AllNoOfItems == null)
            {
                noOfAllNote = 0;
            }
            else
            {
                var setAllAllNoOfItems = from k in SterlingService.Current.Database.Query<Notes, int>()
                                      select k.LazyValue;
                noOfAllNote = setAllAllNoOfItems.Count();
            }


            return noOfAllNote;
        }


        public int noOfOverDueNote;
        public object countOverItem()
        {

            var AllOverNoOfItems = (from k in SterlingService.Current.Database.Query<Notes, int>()
                                select k.LazyValue.Value).FirstOrDefault();


            if (AllOverNoOfItems == null)
            {
                noOfOverDueNote = 0;
            }
            else
            {
                var setOverAllAllNoOfItems = (from k in SterlingService.Current.Database.Query<Notes, int>()
                                              where k.LazyValue.Value.DueDate >= DateTime.Now
                                              where k.LazyValue.Value.dueCompleted == 0
                                                select k.LazyValue);
                noOfOverDueNote = setOverAllAllNoOfItems.Count();
            }


            return noOfOverDueNote;
        }
    }
}
