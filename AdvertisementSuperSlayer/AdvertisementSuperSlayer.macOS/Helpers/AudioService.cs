using System;
using System.Threading.Tasks;
using AdvertisementSuperSlayer.Helpers;
using Xamarin.Forms;
using AdvertisementSuperSlayer.macOS.Helpers;
using AVFoundation;
using Foundation;
using System.IO;

[assembly: Dependency(typeof(AudioService))]
namespace AdvertisementSuperSlayer.macOS.Helpers
{
    public class AudioService : IAudio, IDisposable
    {
        private AVAudioPlayer Player;
        public AudioService() { }

        public async Task PlayAudioFile(string filename)
        {
            string sFilePath = NSBundle.MainBundle.PathForResource(
                Path.GetFileNameWithoutExtension(filename),
                Path.GetExtension(filename));
            NSUrl url = NSUrl.FromString(sFilePath);
            Player = AVAudioPlayer.FromUrl(url);
            Player.FinishedPlaying += (s, e) => { Player = null; };
            await Task.Run(() => Player.Play());
        }

        public void PlaySound()
        {
            Player?.Play();
        }

        public async Task PlaySoundAsync()
        {
            await Task.Run(() => Player?.Play());
        }

        public void SetupAudioFile(string filename)
        {
            string sFilePath = NSBundle.MainBundle.PathForResource(
                Path.GetFileNameWithoutExtension(filename),
                Path.GetExtension(filename));
            NSUrl url = NSUrl.FromString(sFilePath);
            Player = AVAudioPlayer.FromUrl(url);
        }

        public void SetupAudioFile(string filename, bool loop, double volume)
        {
            string sFilePath = NSBundle.MainBundle.PathForResource(
                Path.GetFileNameWithoutExtension(filename),
                Path.GetExtension(filename));
            NSUrl url = NSUrl.FromString(sFilePath);
            Player = AVAudioPlayer.FromUrl(url);
            Player.NumberOfLoops = new nint(Convert.ToInt32(loop));
            Player.Volume = (float)volume;
        }

        public void Dispose()
        {
            Player.Dispose();
            Player = null;
        }

        ~AudioService()
        {
            Dispose();
        }
    }
}
