using System;
using System.Linq;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using SkiaSharp.Waveform;
using SoundMachine.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;

namespace SoundMachine.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SoundPlayer
    {
        private bool _hasLoaded;
        private Waveform _waveForm;

        public SoundPlayer()
        {
            InitializeComponent();
        }

        public SoundPlayerViewModelBase ViewModel { get; private set; }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            ViewModel = BindingContext as SoundPlayerViewModelBase;

            if (ViewModel != null)
            {
                ViewModel.PlayStarted += ViewModel_PlayStarted;
            }
        }

        private void ViewModel_PlayStarted(object sender, EventArgs e)
        {
            if (_waveForm == null && ViewModel is WaveFormPlayerViewModel waveFormVm)
            {
                var sampleCount = DeviceInfo.Idiom == DeviceIdiom.Phone ? 200 : 500;

                var amplitudes = waveFormVm.GetAmplitudes(sampleCount).ToArray();

                _waveForm = new Waveform.Builder()
                    .WithAmplitudes(amplitudes)
                    .WithSpacing(5f)
                    .WithColor(new SKColor(0x00, 0x00, 0xff))
                    .Build();

                WaveformCanvas.InvalidateSurface();
            }
        }

        private void PaintWaveFormCanvas(object sender, SKPaintSurfaceEventArgs e)
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