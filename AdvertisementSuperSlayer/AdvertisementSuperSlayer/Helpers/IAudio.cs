using System.Threading.Tasks;

namespace AdvertisementSuperSlayer.Helpers
{
    public interface IAudio
    {
        Task PlayAudioFile(string filename);

        void SetupAudioFile(string filename);

        void SetupAudioFile(string filename, bool loop, double volume);

        void PlaySound();
    }
}
