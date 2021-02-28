using System.IO;
using System.Linq;

namespace SoundMachine.ViewModels
{
    public class FilePlayerViewModel : SoundPlayerViewModelBase
    {
        public FilePlayerViewModel(string filePath, string displayName, string groupName)
            : base(displayName, groupName)
        {
            FilePath = filePath;

            EnsurePlayerLoaded();
        }

        public string FilePath { get; }

        public override Stream GetAudioStream()
        {
            var stream = GetType().Assembly.GetManifestResourceStream(FilePath);

            return stream;
        }
    }
}