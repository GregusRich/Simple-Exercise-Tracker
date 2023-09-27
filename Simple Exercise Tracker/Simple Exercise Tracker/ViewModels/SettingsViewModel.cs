using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Simple_Exercise_Tracker.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> AvailableBackgroundColors { get; set; }
        public ObservableCollection<string> AvailableTextColors { get; set; }


        private readonly MainPageViewModel mainPageViewModel;
        
        public event PropertyChangedEventHandler PropertyChanged;

        private string _backgroundColor;
        public string BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                if (_backgroundColor != value)
                {
                    _backgroundColor = value;
                    OnPropertyChanged(nameof(BackgroundColor));
                    SaveSettings();
                }
            }
        }

        private string _textColor;
        public string TextColor
        {
            get => _textColor;
            set
            {
                if (_textColor != value)
                {
                    _textColor = value;
                    OnPropertyChanged(nameof(TextColor));
                    SaveSettings();
                }
            }
        }

        private double _exerciseTimePerDay = 30;
        public double ExerciseTimePerDay
        {
            get => _exerciseTimePerDay;
            set
            {
                if (_exerciseTimePerDay != value)
                {
                    _exerciseTimePerDay = value;
                    OnPropertyChanged(nameof(ExerciseTimePerDay));
                    SaveSettings();
                }
            } 
        }
        public ICommand ClearDataCommand { get; }
        public ICommand SaveSettingsCommand { get; }
        public ICommand NavigateBackCommand { get; set; } // Go back to MainPage


        public SettingsViewModel(MainPageViewModel mainPageViewModel)
        {
            this.mainPageViewModel = mainPageViewModel;

            System.Diagnostics.Debug.WriteLine($"Before LoadSettings: ExerciseTimePerDay = {ExerciseTimePerDay}");

            // Load existing settings
            LoadSettings();

            System.Diagnostics.Debug.WriteLine($"After LoadSettings: ExerciseTimePerDay = {ExerciseTimePerDay}");


            ClearDataCommand = mainPageViewModel.ClearDataCommand;
            SaveSettingsCommand = new Command(SaveSettings);
            NavigateBackCommand = new Command(async () => await NavigateBack());
            
            AvailableBackgroundColors = new ObservableCollection<string>
            {"AliceBlue", "Blue", "DarkGray", "Gray", "LightGray", "LightSkyBlue", "PaleTurquoise", "Pink", "PowderBlue", "White"};

            AvailableTextColors = new ObservableCollection<string>
            {"Black", "DarkBlue", "Red", "White"};
        }

        private async Task NavigateBack()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private void LoadSettings()
        {
            BackgroundColor = Preferences.Get("background_color", "White");
            TextColor = Preferences.Get("text_color", "Black");
            ExerciseTimePerDay = Preferences.Get("exercise_time_per_day", 30.0);
        }

        private void SaveSettings() // Method for saving the users settings preferences

        {
            Preferences.Set("background_color", BackgroundColor);
            Preferences.Set("text_color", TextColor);
            Preferences.Set("exercise_time_per_day", ExerciseTimePerDay);

            mainPageViewModel.RefreshPreferences();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
