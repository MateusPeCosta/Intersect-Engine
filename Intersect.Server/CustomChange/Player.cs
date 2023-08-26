namespace Intersect.Server.Entities
{
    public partial class Player : Entity
    {
        public long ExpModifiedByLevel(int enemyLevel, long exp, int playerLevel = 0)
        {
            var expMultiplier = 1f;
            var levelDiff = (playerLevel == 0 ? Level : playerLevel) - enemyLevel;

            if (levelDiff >= 4 && levelDiff < 6)
            {
                expMultiplier = 0.8f;
            }
            else if (levelDiff >= 6 && levelDiff < 10)
            {
                expMultiplier = 0.6f;
            }
            else if (levelDiff >= 10)
            {
                expMultiplier = 0.2f;
            }

            return (long)(expMultiplier * exp);
        }
    }
}
