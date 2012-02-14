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
using Wintellect.Sterling.Database;
using Wintellect.Sterling.Keys;
using Wintellect.Sterling;
using Wintellect.Sterling.Indexes;
using System.Linq;
using System.Diagnostics;

namespace Zub_App
{
    public class IdentityTrigger<T> : BaseSterlingTrigger<T, int> where T : class, IBaseModel, new()
    {
        private static int _idx = 1;

		public IdentityTrigger(ISterlingDatabaseInstance database)
		{
			// if a record exists, set it to the highest value plus 1  
			if (database.Query<T, int>().Any())
			{
				_idx = database.Query<T, int>().Max(key => key.Key) + 1;
			}

            
		}


        public override bool BeforeSave(T instance)
		{
			if (instance.Id < 1)
			{
				instance.Id = _idx++;
			}

			return true;
		}

		public override void AfterSave(T instance)
		{
			return;
		}

		public override bool BeforeDelete(int key)
		{
			return true;
		}

    }

    public interface IBaseModel
    {
        int Id { get; set; }
    }
}
