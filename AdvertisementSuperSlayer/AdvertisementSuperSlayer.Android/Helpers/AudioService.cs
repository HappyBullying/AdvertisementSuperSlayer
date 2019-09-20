using System.Threading.Tasks;
using AdvertisementSuperSlayer.Helpers;
using Xamarin.Forms;
using AdvertisementSuperSlayer.Droid.Helpers;
using Android.Media;
using Android.Content.Res;
using System;

[assembly: Dependency(typeof(AudioService))]
namespace AdvertisementSuperSlayer.Droid.Helpers
{
    public class AudioService : IAudio, IDisposable
    {
        private MediaPlayer Player;
        public AudioService() { Player = new MediaPlayer(); }


        public async Task PlayAudioFile(string filename)
        {
            var fd = Android.App.Application.Context.Assets.OpenFd(filename);
            Player.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
            Player.Prepared += (s, e) => Player.Start();
            await Task.Run(() => Player.PrepareAsync());
        }

        public void SetupAudioFile(string filename)
        {
            AssetFileDescriptor fd = Android.App.Application.Context.Assets.OpenFd(filename);
            Player.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
            Player.Prepare();
        }

        public void SetupAudioFile(string filename, bool loop, double volume)
        {
            AssetFileDescriptor fd = Android.App.Application.Context.Assets.OpenFd(filename);
            Player.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
            float all_v = (float)volume;
            Player.SetVolume(all_v, all_v);
            Player.Looping = loop;
            Player.Prepare();
        }

        public void PlaySound()
        {
            Player?.Start();
        }

        public void Dispose()
        {
            this.Player = null;
        }
    }
}