using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace WrackEngine
{
    [Serializable]
    public class Animation
    {
        public static Dictionary<string, Dictionary<string, Animation>> AnimationSets = new Dictionary<string, Dictionary<string, Animation>>();

        public static Dictionary<string, Animation> GetAnimationSet(string name)
        {
            if (AnimationSets.ContainsKey(name)) return AnimationSets[name];
            else return null;
        }
        public static Dictionary<string, Animation> GetAnimationSetClone(string name)
        {
            if (!AnimationSets.ContainsKey(name)) return null;
            Dictionary<string, Animation> s = AnimationSets[name];
            Dictionary<string, Animation> d = new Dictionary<string, Animation>();
            for (int i = 0; i < s.Count; i++)
            {
                KeyValuePair<string, Animation> kvp = s.ElementAt(i);
                d.Add(kvp.Key, kvp.Value.DeepClone());
            }
            return d;
        }

        public static void AddAnimationSet(string name, Dictionary<string, Animation> animationSet)
        {
            AnimationSets.Add(name, animationSet);
        }

        public int Frames { get; set; }
        public int StartingFrame { get; set; }
        public int FrameTime { get; set; }
        public bool Swings { get; set; }
        [ContentSerializerIgnore]
        public bool IsSwinging { get; set; }

        public Animation()
        {
            Frames = 1;
            StartingFrame = 0;
            FrameTime = (int)((1f / 60f) * 1000);
            Swings = false;
            IsSwinging = false;
        }

        public virtual Animation DeepClone()
        {
            Animation a = new Animation();
            a.Frames = Frames;
            a.StartingFrame = StartingFrame;
            a.FrameTime = FrameTime;
            a.Swings = Swings;
            a.IsSwinging = IsSwinging;
            return a;
        }

        public static Animation FromLoaderData(int frames, int startingFrame, int frameTime, bool swings)
        {
            Animation a = new Animation();
            a.Frames = frames;
            a.StartingFrame = startingFrame;
            a.FrameTime = frameTime;
            a.Swings = swings;
            a.IsSwinging = false;
            return a;
        }
    }
}
