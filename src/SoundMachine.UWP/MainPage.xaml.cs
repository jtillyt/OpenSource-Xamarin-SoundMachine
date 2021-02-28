using System;
using System.Linq;

namespace SoundMachine.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new SoundMachine.App());
        }
    }
}