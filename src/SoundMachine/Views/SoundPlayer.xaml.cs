using SkiaSharp;
using SkiaSharp.Waveform;
using SoundMachine.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace SoundMachine.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SoundPlayer
    {
        private Waveform _waveForm;
        private bool _hasLoaded = false;

        public SoundPlayer()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            ViewModel = BindingContext as SoundPlayerViewModelBase;

            if (ViewModel != null)
            {
                ViewModel.PlayStarted += ViewModel_PlayStarted;
            }
        }

        private void ViewModel_PlayStarted(object sender, System.EventArgs e)
        {
            if (_waveForm == null && ViewModel is WaveFormPlayerViewModel waveFormVm)
            {
                int sampleCount = DeviceInfo.Idiom == DeviceIdiom.Phone ? 200 : 500;

                var amplitudes = waveFormVm.GetAmplitudes(sampleCount).ToArray();

                _waveForm = new Waveform.Builder()
                    .WithAmplitudes(amplitudes)
                    .WithSpacing(5f)
                    .WithColor(new SKColor(0x00, 0x00, 0xff))
                    .Build();

                WaveformCanvas.InvalidateSurface();
            }
        }

        public SoundPlayerViewModelBase ViewModel { get; private set; }

        private void PaintWaveFormCanvas(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            if (_waveForm != null && !_hasLoaded)
            {
                //We only need to do this once
                _hasLoaded = true;

                _waveForm.DrawOnCanvas(e.Surface.Canvas);
            }
        }
    }
}