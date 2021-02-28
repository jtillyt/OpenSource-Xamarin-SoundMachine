using System.IO;

namespace SoundMachine.ViewModels
{
    public class FilePlayerViewModel : SoundPlayerViewModelBase
    {
        private readonly string _filePath;

        public FilePlayerViewModel(string filePath, string displayName, string groupName)
            :base(displayName, groupName)
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
