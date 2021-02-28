using System.Linq;
using SoundMachine.Views;
using Xamarin.Forms;

namespace SoundMachine
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new SoundMachinePage();
        }
    }
}