using Simple_Exercise_Tracker.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Simple_Exercise_Tracker
{
    public partial class MainPage : ContentPage
    {
        private static MainPageViewModel _mainPageViewModel;

        public MainPage()
        {
            InitializeComponent();

            if (_mainPageViewModel == null)
            {
                _mainPageViewModel = new MainPageViewModel();
            }

            this.BindingContext = _mainPageViewModel;
        }
    }
}
