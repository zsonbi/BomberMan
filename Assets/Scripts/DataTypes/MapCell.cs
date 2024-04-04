namespace DataTypes
{
    public enum MapCell : byte
    {
        Walkable = 0,
        DestructibleWall = 1,
        IndestructibleWall = 2,
        PlayerSpawn = 3,
        MonsterSpawn = 4,
    }
}