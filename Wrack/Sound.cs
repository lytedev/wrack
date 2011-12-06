using System.Collections.Generic;

using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Wrack
{
    public static class Sound
    {
        private static Dictionary<string, SoundEffect> Sounds = new Dictionary<string, SoundEffect>();
        private static Dictionary<string, Song> Songs = new Dictionary<string, Song>();

        // Sound Functions
        public static void PlaySound(string name, bool loop = false)
        {
            SoundEffectInstance sfi = Sounds[name].CreateInstance();
            sfi.IsLooped = loop;
            sfi.Play();
        }

        public static void AddSound(string name, SoundEffect sound)
        {
            Sounds.Add(name, sound);
        }

        public static SoundEffect GetSound(string name)
        {
            return Sounds[name];
        }

        public static void ClearSounds()
        {
            Sounds.Clear();
        }

        // Song Functions
        public static void PlaySong(string name, bool loop = false)
        {
            MediaPlayer.Play(Songs[name]);
            MediaPlayer.IsRepeating = loop;
        }

        public static void AddSong(string name, Song song)
        {
            Songs.Add(name, song);
        }

        public static Song GetSong(string name)
        {
            return Songs[name];
        }

        public static void ClearSongs()
        {
            Songs.Clear();
        }
    }
}
