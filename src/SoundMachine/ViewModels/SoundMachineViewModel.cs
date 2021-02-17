using ReactiveUI;
using System.Collections.ObjectModel;

namespace SoundMachine.ViewModels
{
    public class SoundMachineViewModel : ReactiveObject
    {
        public SoundMachineViewModel()
        {
            _soundPlayers = new ObservableCollection<SoundPlayerViewModelBase>()
            {
                new FilePlayerViewModel("SoundMachine.Audio.PositiveShortDigital.flac", "Positive Sound"),
                new FilePlayerViewModel("SoundMachine.Audio.NegativeShortDigital.flac", "Negative Sound"),
                new FilePlayerViewModel("SoundMachine.Audio.BeachBird.flac", "Beach Birds"),
                new FilePlayerViewModel("SoundMachine.Audio.UrbanBird.flac", "Urban Birds"),
            };
        }

        private ObservableCollection<SoundPlayerViewModelBase> _soundPlayers;
        public ObservableCollection<SoundPlayerViewModelBase> SoundPlayers
        {
            get => _soundPlayers;
            set => this.RaiseAndSetIfChanged(ref _soundPlayers, value);
        }
    }
}
