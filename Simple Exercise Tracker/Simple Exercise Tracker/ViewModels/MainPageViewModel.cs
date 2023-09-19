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

        // Accessor for total minutes exercised (used in converstion for total hours exercised)
        private double _totalMinsExercised;
        public double TotalMinsExercised
        {
            get => _totalMinsExercised;
            set
            {
                if (_totalMinsExercised != value)
                {
                    _totalMinsExercised = value;
                    OnPropertyChanged();
                }
            }
        }


        // Accessor for Minutes Exercised
        private double _minutesExercised;
        public double MinutesExercised
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

        private double _averageMinsExerciseNeeded;
        public double AverageMinsExerciseNeeded
        {
            get => _averageMinsExerciseNeeded;
            set
            {
                _averageMinsExerciseNeeded = value;
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

        // Accessor for Hours Exercised
        private string _hoursExercised;
        public string HoursExercised
        {
            get => _hoursExercised;
            set
            {
                if (_hoursExercised != value)
                {
                    _hoursExercised = value;
                    OnPropertyChanged();
                }
            }
        }

        // Accessor for Hours Should Have Exercised
        private string _hoursShouldHaveExercised;
        public string HoursShouldHaveExercised
        {
            get => _hoursShouldHaveExercised;
            set
            {
                _hoursShouldHaveExercised = value;
                OnPropertyChanged();
            }
        }


        public void ClearData() // Clears exercise logs
        {
            ExerciseLogs.Clear();
            MinutesExercised = 0;
        }


        // Calculate the Average Minutes Exercised for the year so far
        public void CalculateAverageMinutesExercised()
        {
            if (ExerciseLogs.Count == 0) return;  // Prevent division by zero

            _totalMinsExercised = ExerciseLogs.Sum(log => log.MinutesExercised);

            // Calculate the number of days since the start of the year to today
            DateTime startOfYear = new DateTime(DateTime.Now.Year, 1, 1);
            double daysSinceStartOfYear = (DateTime.Now - startOfYear).Days + 1;

            AverageMinutesExercised = Math.Round(_totalMinsExercised / daysSinceStartOfYear);

            // Changes the background colour if they have met the 30mins workout goal
            AverageExerciseColour = AverageMinutesExercised >= 30 ? Color.LightGreen : Color.IndianRed;

            CalculateHoursExercised(); // Updates the hours exercised
        }


        // Calculate the average amount of minutes per day needed to hit the 30min goal
        public void CalculateAverageMinutesExerciseNeeded()
        {
            if (ExerciseLogs != null)
            {
                // Calculate the total minutes needed for the current year
                double totalMinsNeededInYear = (DateTime.IsLeapYear(DateTime.Now.Year) ? 366 : 365) * 30;

                // Calculate the total minutes remaining to reach the year's goal
                double totalMinsRemainingInYear = totalMinsNeededInYear - _totalMinsExercised;

                // Calculate remaining days in the year
                int remainingDays = (new DateTime(DateTime.Now.Year, 12, 31) - DateTime.Now).Days;

                AverageMinsExerciseNeeded = Math.Round(totalMinsRemainingInYear / remainingDays);
            }
        }

        // Calculates the hours exercised using the total minutes exercised
        public void CalculateHoursExercised()
        {
            HoursExercised = ConvertMinutesToHoursAndMinutes(_totalMinsExercised);
            Debug.WriteLine($"Hours exercised: {HoursExercised}");
        }


        // Converts minutes to hours and minutes and returns a string representation. 
        public string ConvertMinutesToHoursAndMinutes(double minutes)
        {
            double hours = Math.Floor(minutes / 60);
            double mins = minutes % 60;
            return $"{hours} hours and {Math.Round(mins, 2)} minutes:";
        }


        // Calculates the hours the user should have exercised
        public void CalculateHoursShouldHaveExercised()
        {
            double daysSinceStartOfYear = (DateTime.Now - new DateTime(DateTime.Now.Year, 1, 1)).Days + 1;
            double totalMinsShouldHaveExercised = daysSinceStartOfYear * 30;

            HoursShouldHaveExercised = ConvertMinutesToHoursAndMinutes(totalMinsShouldHaveExercised);
        }

        // Submits the minutes exercised command: Linked to the submit button on the MainPage
        public ICommand SubmitMinutesExercisedCommand { get; set; }

        // Submit the clear data command
        public ICommand ClearDataCommand { get; set; }



        // Initialise MainPageViewModel
        public MainPageViewModel()
        {
            TodayDate = DateTime.Now.ToString("dd-MM-yyyy");

            // Initialise the clear submit minutes exercised command
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
                Debug.WriteLine($"You have exercised for an avereage of {AverageMinutesExercised} minutes per day!");

                CalculateAverageMinutesExerciseNeeded();
                Debug.Write($"You will need to exercise for an average of {AverageMinsExerciseNeeded} minutes per day to meet your goal\n");
            });

            // Initialise the clear data command
            ClearDataCommand = new Command(() => ClearData());
        }

        // OnPropertyChanged Event Handler 
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
