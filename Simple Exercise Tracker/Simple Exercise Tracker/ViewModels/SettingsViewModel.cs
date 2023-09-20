using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;



namespace Simple_Exercise_Tracker.ViewModels
{
    public class SettingsViewModel
    {
        public string BackgroundColor { get; set; }
        public string TextColor { get; set; }
        public double ExerciseTimePerDay { get; set; }
        // Commands for clearing data and saving settings
        public ICommand ClearDataCommand { get; }
        public ICommand SaveSettingsCommand { get; }

        public SettingsViewModel(MainPageViewModel mainPageViewModel)
        {
            // Load existing settings
            LoadSettings();

            ClearDataCommand = mainPageViewModel.ClearDataCommand;
            SaveSettingsCommand = new Command(SaveSettings);
        }

        private void LoadSettings()
        {
            BackgroundColor = Preferences.Get("background_color", "default_color");
            TextColor = Preferences.Get("text_color", "default_color");
            ExerciseTimePerDay = Preferences.Get("exercise_time_per_day", 30.0);
        }

        private void SaveSettings() // Method for saving the users settings preferences

        {
            Preferences.Set("background_color", BackgroundColor);
            Preferences.Set("text_color", TextColor);
            Preferences.Set("exercise_time_per_day", ExerciseTimePerDay);
        }

        public void ChangeTextColour(string newColor)
        {
            TextColor = newColor;
            SaveSettings();
        }

        public void ChangeExerciseTimePerDay(double newTime)
        {
            ExerciseTimePerDay = newTime;
            SaveSettings();
        }
    }
}
