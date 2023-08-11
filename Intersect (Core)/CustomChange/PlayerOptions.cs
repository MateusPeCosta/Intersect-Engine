namespace Intersect.Config
{
    /// <summary>
    /// Contains configurable options pertaining to the way Players are handled by the engine.
    /// </summary>
    public partial class PlayerOptions
    {
        /// <summary>
        /// Default value for the percentage by which the player's movement speed is increased.
        /// </summary>
        public int RunningSpeedPercent { get; set; } = 20; //Increased speed by pressing the button.

    }
}