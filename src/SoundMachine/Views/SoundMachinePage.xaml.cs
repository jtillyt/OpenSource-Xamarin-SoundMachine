using SoundMachine.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SoundMachine.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SoundMachinePage : ContentPage
    {
        public SoundMachinePage()
        {
            InitializeComponent();

            ViewModel = new SoundMachineViewModel();
            BindingContext = ViewModel;
        }

        public SoundMachineViewModel ViewModel { get; }
    }
}