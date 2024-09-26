public class GlobalEnum
{
    public enum Direction
    {
        NORTH=0,
        EAST=1,
        SOUTH=2,
        WEST=3
    }

    public static Direction inverseDirection(Direction direction)
    {
        if(direction == Direction.NORTH)
        {
            return Direction.SOUTH;
        }
        else if(direction == Direction.EAST)
        {
            return Direction.WEST;
        }
        else if (direction == Direction.SOUTH)
        {
            return Direction.NORTH;
        }
        else { return Direction.EAST; }
    }
}