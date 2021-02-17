using System.IO;

namespace SoundMachine.ViewModels
{
    public class FilePlayerViewModel : SoundPlayerViewModelBase
    {
        private readonly string _filePath;

        public FilePlayerViewModel(string filePath, string displayName)
            :base(displayName)
        {
            _filePath = filePath;

            EnsurePlayerLoaded();
        }

        public override Stream GetAudioStream()
        {
            var stream = GetType().Assembly.GetManifestResourceStream(_filePath);

            return stream;
        }
    }
}
