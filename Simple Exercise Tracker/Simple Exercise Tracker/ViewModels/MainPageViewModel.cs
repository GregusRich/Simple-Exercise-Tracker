using Simple_Exercise_Tracker.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;


namespace Simple_Exercise_Tracker.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        // Record the exercise logs in an observable collection
        private ObservableCollection<ExerciseLog> _exerciseLogs = new ObservableCollection<ExerciseLog>();
        public ObservableCollection<ExerciseLog> ExerciseLogs
        {
            get => _exerciseLogs;
            set
            {
                _exerciseLogs = value;
                OnPropertyChanged();
            }
        }

        // Accessor for Minutes Exercised
        private int _minutesExercised;
        public int MinutesExercised
        {
            get => _minutesExercised;
            set
            {
                _minutesExercised = value;
                OnPropertyChanged();
            }
        }

        // Accessor for Todays Date
        private string _todayDate;
        public string TodayDate
        {
            get => _todayDate;
            set
            {
                _todayDate = value;
                OnPropertyChanged();
            }
        }

        // Accessor for Average Minutes Exercised 
        private double _averageMinutesExercised;
        public double AverageMinutesExercised
        {
            get => _averageMinutesExercised;
            set
            {
                _averageMinutesExercised = value;
                OnPropertyChanged();
            }
        }

        // Accessor for the Average Exercise Colour (red or green)
        private Color _averageExerciseColour;
        public Color AverageExerciseColour
        {
            get => _averageExerciseColour;
            set
            {
                _averageExerciseColour = value;
                OnPropertyChanged();
            }
        }


        // Calculate the Average Minutes Exercised for the year so far
        public void CalculateAverageMinutesExercised()
        {
            if (ExerciseLogs.Count == 0) return;  // Prevent division by zero

            double totalMinutes = ExerciseLogs.Sum(log => log.MinutesExercised);

            // Calculate the number of days since the start of the year to today
            DateTime startOfYear = new DateTime(DateTime.Now.Year, 1, 1);
            int daysSinceStartOfYear = (DateTime.Now - startOfYear).Days + 1;

            AverageMinutesExercised = Math.Round(totalMinutes / daysSinceStartOfYear);

            // Changes the background colour if they have met the 30mins workout goal
            AverageExerciseColour = AverageMinutesExercised >= 30 ? Color.LightGreen : Color.Red;
        }

        // Submits the Minutes Exercised Command: Linked to the submit button on the MainPage
        public ICommand SubmitMinutesExercisedCommand { get; set; }


        // Initialise MainPageViewModel
        public MainPageViewModel()
        {
            TodayDate = DateTime.Now.ToString("dd-MM-yyyy");

            SubmitMinutesExercisedCommand = new Command(() =>
            {
                var newLog = new ExerciseLog
                {
                    Date = DateTime.Now,
                    MinutesExercised = MinutesExercised
                };

                ExerciseLogs.Add(newLog); // Add new exercise log
                Debug.WriteLine($"You have exercised for {MinutesExercised} minutes today.");
                
                // Calculate and update average
                CalculateAverageMinutesExercised();
                Debug.WriteLine($"You have exercised for an avereage of {AverageMinutesExercised} per day!");
            });
        }

        // OnPropertyChanged Event Handler 
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
