using System;
using System.Threading.Tasks;
using AdvertisementSuperSlayer.Helpers;
using AdvertisementSuperSlayer.UWP.Helpers;
using Windows.ApplicationModel;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioService))]
namespace AdvertisementSuperSlayer.UWP.Helpers
{
    public class AudioService : IAudio, IDisposable
    {
        private MediaPlayer Player;

        public AudioService() { Player = new MediaPlayer(); }

        public async Task PlayAudioFile(string filename)
        {
            StorageFolder folder = await Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            StorageFile file = await folder.GetFileAsync(filename);
            Player.AutoPlay = false;
            Player.IsLoopingEnabled = false;
            Player.Source = MediaSource.CreateFromStorageFile(file);
            Player.Play();
        }

        public void PlaySound()
        {
            Player?.Play();
        }

        public async Task PlaySoundAsync()
        {
            await Task.Run(() => Player?.Play());
        }

        public async void SetupAudioFile(string filename)
        {
            StorageFolder folder = await Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            StorageFile file = await folder.GetFileAsync(filename);
            Player.AutoPlay = false;
            Player.IsLoopingEnabled = false;
            Player.Source = MediaSource.CreateFromStorageFile(file);
        }

        public async void SetupAudioFile(string filename, bool loop, double volume)
        {
            StorageFolder folder = await Package.Current.InstalledLocation.GetFolderAsync(@"Assets");
            StorageFile file = await folder.GetFileAsync(filename);
            Player.AutoPlay = false;
            Player.IsLoopingEnabled = loop;
            Player.Volume = volume;
            Player.Source = MediaSource.CreateFromStorageFile(file);
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
