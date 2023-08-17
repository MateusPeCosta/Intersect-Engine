namespace Intersect.Config
{
    /// <summary>
    /// Contains configurable options pertaining to the way sprites are rendered within the engine
    /// </summary>
    public partial class SpriteOptions
    {
        /// <summary>
        /// Sets the number of frames there will be in the sprite sheets to run.
        /// </summary>
        public int RunningFrames { get; set; } = 4;
    }
}
