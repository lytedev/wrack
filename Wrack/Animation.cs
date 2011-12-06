namespace Wrack
{
    public class Animation
    {
        public int Frames { get; set; }
        public int StartingFrame { get; set; }
        public int FrameTime { get; set; }
        public bool Swings { get; set; }
        public bool IsSwinging { get; set; }

        public void Default()
        {
            Frames = 1;
            StartingFrame = 0;
            FrameTime = (int)((1f / 60f) * 1000);
            Swings = false;
            IsSwinging = false;
        }

        public Animation()
        {
            Default();
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
    }
}
