using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AudioX
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        List<ISimpleAudioPlayer> Sound = new List<ISimpleAudioPlayer>();
        double C = Math.Pow(2, 1 / 12.0);
        int S = 0;

        public MainPage()
        {
            InitializeComponent();
            Sound.Add(CrossSimpleAudioPlayer.CreateSimpleAudioPlayer());
            Sound.Add(CrossSimpleAudioPlayer.CreateSimpleAudioPlayer());
            Device.StartTimer(new TimeSpan(0,0,0,0,100), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    OnTimer();
                });
                return true; // runs again, or false to stop
            });
        }

        double oldVal = -1;
        double oldVal2 = -1;

        int AddAudio(Stream stream, double volume = 1)
        {
            Sound.Add(CrossSimpleAudioPlayer.CreateSimpleAudioPlayer());
            int res = Sound.Count - 1;

            Sound[res].Load(stream);
            Sound[res].Volume = volume;
            return res;
        }


        public void PlayBeep(UInt16 frequency, int msDuration, UInt16 volume = 16383)
        {
            var mStrm = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(mStrm);

            const double TAU = 2 * Math.PI;
            int formatChunkSize = 16;
            int headerSize = 8;
            short formatType = 1;
            short tracks = 1;
            int samplesPerSecond = 44100;
            short bitsPerSample = 16;
            short frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
            int bytesPerSecond = samplesPerSecond * frameSize;
            int waveSize = 4;
            int samples = (int)((decimal)samplesPerSecond * msDuration / 1000);
            int dataChunkSize = samples * frameSize;
            int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;
            // var encoding = new System.Text.UTF8Encoding();
            writer.Write(0x46464952); // = encoding.GetBytes("RIFF")
            writer.Write(fileSize);
            writer.Write(0x45564157); // = encoding.GetBytes("WAVE")
            writer.Write(0x20746D66); // = encoding.GetBytes("fmt ")
            writer.Write(formatChunkSize);
            writer.Write(formatType);
            writer.Write(tracks);
            writer.Write(samplesPerSecond);
            writer.Write(bytesPerSecond);
            writer.Write(frameSize);
            writer.Write(bitsPerSample);
            writer.Write(0x61746164); // = encoding.GetBytes("data")
            writer.Write(dataChunkSize);
            {
                double theta = frequency * TAU / (double)samplesPerSecond;
                // 'volume' is UInt16 with range 0 thru Uint16.MaxValue ( = 65 535)
                // we need 'amp' to have the range of 0 thru Int16.MaxValue ( = 32 767)
                double amp = volume >> 2; // so we simply set amp = volume / 2
                for (int step = 0; step < samples; step++) {
                    short s = (short)(amp * Math.Sin(theta * (double)step));
                    writer.Write(s);
                }
            }

            writer.Close();
            mStrm.Close();

            //AddAudio(new MemoryStream(mStrm.ToArray()));
            Sound[S].Load(new MemoryStream(mStrm.ToArray()));
            Sound[S].Play();
            S = 1 - S;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            PlayBeep(CalcFreq(Freq.Value), 1000);
            PlayBeep(CalcFreq(Freq2.Value), 1000);
        }

        private void OnTimer()
        {
            if (Freq.Value != oldVal) {
                oldVal = Freq.Value;
                PlayBeep(CalcFreq(Freq.Value), 100);
            }
            if (Freq2.Value != oldVal2) {
                oldVal2 = Freq2.Value;
                PlayBeep(CalcFreq(Freq2.Value), 200);
            }
        }

        ushort CalcFreq(double v)
        {
            return (ushort)(262 * Math.Pow(C, (v - 40)+20));
        }

        private void Freq_ValueChanged(object sender, ValueChangedEventArgs e)
        {
        }
    }
}
