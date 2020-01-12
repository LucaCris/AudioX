using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Media;

namespace AudioX.Droid
{
    [Activity(Label = "AudioX", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            /*
            SoundSetup();
            Play(2093, 1000);
            Play(2349, 1000);
            Play(2637, 1000);
            Play(2794, 1000);
            Play(3136, 1000);

            Play(131, 1000);
            Play(147, 1000);
            Play(165, 1000);
            Play(175, 1000);
            Play(196, 1000);
            */
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        /*
        AudioTrack m_Track = null;
        int m_SampleRate = 8000;
        int m_Tone = 2093;

        public void SoundSetup()
        {
            var duration = 1;
            var numSamples = duration * m_SampleRate;
            var sample = new double[numSamples];
            byte[] generatedSnd = new byte[2 * numSamples];

            for (int i = 0; i < numSamples; ++i) {
                sample[i] = Math.Sin(2 * Math.PI * i / (m_SampleRate / m_Tone));
            }

            int idx = 0;
            foreach (double dVal in sample) {
                short val = (short)(dVal * 32767);
                generatedSnd[idx++] = (byte)(val & 0x00ff);
                generatedSnd[idx++] = (byte)((val & 0xff00) >> 8);
            }

            //            m_Track = new AudioTrack(Stream.Music, m_SampleRate, ChannelOut.Mono, Encoding.Pcm16bit, numSamples, AudioTrackMode.Static);
            var aa = new AudioAttributes.Builder()
             .SetUsage(AudioUsageKind.Game)
             .SetContentType(AudioContentType.Music)
             .Build();

            var af = new AudioFormat.Builder()
                .SetSampleRate(m_SampleRate)
                .SetChannelMask(ChannelOut.Mono)
                .SetEncoding(Encoding.Pcm16bit)
                .Build();
            m_Track = new AudioTrack(aa, af, numSamples, AudioTrackMode.Static, 0);
            m_Track.Write(generatedSnd, 0, numSamples);
            m_Track.SetLoopPoints(0, m_SampleRate / 2, -1);
        }

        public void Play(int freq, int duration)
        {
            m_Track.SetPlaybackRate(m_SampleRate * freq / m_Tone);
            m_Track.Play();
            System.Threading.Tasks.Task.Delay(duration).Wait();
            m_Track.Stop();
            m_Track.ReloadStaticData();
        }
        */
    }
}