using System;

using Foundation;
using AdvertisementSuperSlayer.Helpers;
using System.Threading.Tasks;
using System.IO;
using AVFoundation;
using Xamarin.Forms;
using AdvertisementSuperSlayer.iOS.Helpers;

[assembly: Dependency(typeof(AudioService))]
namespace AdvertisementSuperSlayer.iOS.Helpers
{
    class AudioService : IAudio, IDisposable
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
            await Task.Run( () => Player.Play());
        }

        public void PlaySound()
        {
            Player?.Play();
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
            this.Player = null;
        }
    }
}