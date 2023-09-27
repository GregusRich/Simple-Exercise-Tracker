using Simple_Exercise_Tracker.ViewModels;
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
    }
}