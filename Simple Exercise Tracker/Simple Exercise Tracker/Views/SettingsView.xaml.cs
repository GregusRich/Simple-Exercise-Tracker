using Simple_Exercise_Tracker.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Simple_Exercise_Tracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : ContentPage
    {
        public SettingsView(SettingsViewModel settingsViewModel)
        {
            InitializeComponent();
            this.BindingContext = settingsViewModel;
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            double roundedValue = Math.Round(e.NewValue / 5) * 5;
            if (roundedValue < 5) roundedValue = 5;  // Ensures the value is at least 5
            ((SettingsViewModel)BindingContext).ExerciseTimePerDay = roundedValue;
        }
    }
}
