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
        public ObservableCollection<string> AvailableBackgroundColors { get; set; } // A list of Available Background Colours
        public ObservableCollection<string> AvailableTextColors { get; set; } // Available Text Colours

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

        /* Bug fix comments:
         * 
         * Very strange bug with the slider. I wanted the user to only be able to move the slider in 5min increments starting at a min goal of 5mins.
         * 
         * Therefor:
         * - Min Value of ExerciseTimePerDay for the slider is 5mins
         * - Max Value ExerciseTimePerDay: 60min
         * 
         * Bug occured when navigating back to home from settings:
         * - Slider gets destroyed therefor setting the Max value to 0
         * - This made:
         *      Max Value 60 (0 when slider is destroyed) 
         *      become lower than the Min Value (5mins)
         * 
         * Max < Min = Frozen app. Fixed by saying if value is lower than 5 value = 5. 
         * 
         */
        private double _exerciseTimePerDay = 30;
        public double ExerciseTimePerDay
        {
            get => _exerciseTimePerDay;
            set
            {
                if (value < 5) value = 5;
                if (value > 60) value = 60;

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
        public ICommand SaveSettingsButtonCommand { get; set; }
        public ICommand NavigateBackCommand { get; set; } // Go back to MainPage

        public SettingsViewModel(MainPageViewModel mainPageViewModel)
        {
            // Load existing settings
            this.mainPageViewModel = mainPageViewModel;

            LoadSettings(); // Load settings from preferences 

            // Sets up all the commands
            ClearDataCommand = mainPageViewModel.ClearDataCommand;
            SaveSettingsCommand = new Command(SaveSettings);
            SaveSettingsButtonCommand = new Command(SaveSettingsButtonClick);
            NavigateBackCommand = new Command(async () => await NavigateBack());
            
            // List of the background colours
            AvailableBackgroundColors = new ObservableCollection<string>
            {"AliceBlue", "Blue", "DarkGray", "Gray", "LightGray", "LightSkyBlue", "PaleTurquoise", "Pink", "PowderBlue", "White"};

            // List of the text colours
            AvailableTextColors = new ObservableCollection<string>
            {"Black", "DarkBlue", "Red", "White"};
        }

        // Navigate back, linked ot the back button icon
        private async Task NavigateBack()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        // Save settings button
        private void SaveSettingsButtonClick()
        {
            SaveSettings();
            Application.Current.MainPage.DisplayAlert("Success", "Settings have been saved.", "OK"); // Show a confirmation message
        }

        // Load settings, used when navigating to the settings page
        public void LoadSettings()
        {
            BackgroundColor = Preferences.Get("background_color", "White");
            TextColor = Preferences.Get("text_color", "Black");
            ExerciseTimePerDay = Preferences.Get("exercise_time_per_day", 30.0);
        }

        // Saves users preferences
        private void SaveSettings() 
        {
            Preferences.Set("background_color", BackgroundColor);
            Preferences.Set("text_color", TextColor);
            Preferences.Set("exercise_time_per_day", ExerciseTimePerDay);
            mainPageViewModel.LoadSettings();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
