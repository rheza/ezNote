using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Wintellect.Sterling;
using Zub_App;


namespace Zub_App
{
    public sealed class SterlingService : IApplicationService, IApplicationLifetimeAware, IDisposable
    {
        
        public const long KILOBYTE = 1024;
        public const long MEGABYTE = 1024 * KILOBYTE;
        public const long QUOTA = 100 * MEGABYTE;
        

        public void Starting()
        {
            DateTime start = DateTime.Now;
            if (DesignerProperties.IsInDesignTool) return;
        
            _engine.Activate();
            
            // Put the type of the Database Class after RegisterDatabase<
            Database = _engine.SterlingDatabase.RegisterDatabase<NotesDatabase>();
            

            SterlingService.Current.Database.RegisterTrigger(new IdentityTrigger<Category>(SterlingService.Current.Database));
            SterlingService.Current.Database.RegisterTrigger(new IdentityTrigger<Notes>(SterlingService.Current.Database));

            
			

            
        }

        
        private SterlingEngine _engine;

        public static SterlingService Current { get; private set; }

       

        public ISterlingDatabaseInstance Database { get; private set; }

        /// <summary>
        /// Called by an application in order to initialize the application extension service.
        /// </summary>
        /// <param name="context">Provides information about the application state. </param>
        public void StartService(ApplicationServiceContext context)
        {
            if (DesignerProperties.IsInDesignTool) return;
			DateTime start = DateTime.Now;
            _engine = new SterlingEngine();
            Current = this;
        }

        /// <summary>
        /// Called by an application in order to stop the application extension service. 
        /// </summary>
        public void StopService()
        {
            return;
        }

        public static void ExecuteOnUIThread(Action action)
        {
            if (Deployment.Current.CheckAccess())
            {
                var dispatcher = Deployment.Current.Dispatcher;
                if (dispatcher.CheckAccess())
                {
                    dispatcher.BeginInvoke(action);
                }
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Called by an application immediately after the <see cref="E:System.Windows.Application.Startup"/> event occurs.
        /// </summary>
        public void Started()
        {
            return;
        }

        /// <summary>
        /// Called by an application immediately before the <see cref="E:System.Windows.Application.Exit"/> event occurs. 
        /// </summary>
        public void Exiting()
        {
            if (DesignerProperties.IsInDesignTool) return;
        }

        /// <summary>
        /// Called by an application immediately after the <see cref="E:System.Windows.Application.Exit"/> event occurs. 
        /// </summary>
        public void Exited()
        {
            Dispose();
            _engine = null;
            return;
        }

        public void RequestRebuild()
        {
            if (RebuildRequested != null)
            {
                RebuildRequested(this, EventArgs.Empty);
            }
        }

        public event EventHandler RebuildRequested;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_engine != null)
            {
                _engine.Dispose();
            }
            GC.SuppressFinalize(this);
        } 
        
    }
    
}
