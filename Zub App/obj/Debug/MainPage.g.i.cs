﻿#pragma checksum "C:\Users\Rheza\Documents\Visual Studio 2010\Projects\Zub App\Zub App\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C843CEE7361FF4577809692DC071851E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Zub_App {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock ListTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.ListBox MainListBox;
        
        internal System.Windows.Controls.TextBlock DueText;
        
        internal System.Windows.Controls.TextBlock DueNo;
        
        internal System.Windows.Controls.TextBlock AllText;
        
        internal System.Windows.Controls.TextBlock AllNo;
        
        internal System.Windows.Controls.Button AllNotesButton;
        
        internal System.Windows.Controls.Button OverdueButton;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton appbar_button1;
        
        internal Microsoft.Phone.Shell.ApplicationBarIconButton appbar_button2;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Zub%20App;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.ListTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ListTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.MainListBox = ((System.Windows.Controls.ListBox)(this.FindName("MainListBox")));
            this.DueText = ((System.Windows.Controls.TextBlock)(this.FindName("DueText")));
            this.DueNo = ((System.Windows.Controls.TextBlock)(this.FindName("DueNo")));
            this.AllText = ((System.Windows.Controls.TextBlock)(this.FindName("AllText")));
            this.AllNo = ((System.Windows.Controls.TextBlock)(this.FindName("AllNo")));
            this.AllNotesButton = ((System.Windows.Controls.Button)(this.FindName("AllNotesButton")));
            this.OverdueButton = ((System.Windows.Controls.Button)(this.FindName("OverdueButton")));
            this.appbar_button1 = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("appbar_button1")));
            this.appbar_button2 = ((Microsoft.Phone.Shell.ApplicationBarIconButton)(this.FindName("appbar_button2")));
        }
    }
}

