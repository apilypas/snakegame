using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SnakeGame;

public class BugSpawner
{
    public IList<Vector2> _locations = [];

    public IList<Vector2> Locations => _locations;

    public BugSpawner()
    {
    }

    public bool Any()
    {
        return _locations.Count > 0;
    }

    public void SpawnAt(Vector2 location)
    {
        _locations.Add(location);
    }

    public bool Kill(Vector2 headLocation)
    {
        var headRectangle = new Rectangle((int)headLocation.X, (int)headLocation.Y, Constants.SegmentSize, Constants.SegmentSize);
        var at = -1;

        for (var i = 0; i < _locations.Count; i++)
        {
            var bug = _locations[i];

            var rectangle = new Rectangle(
                (int)bug.X,
                (int)bug.Y,
                Constants.SegmentSize,
                Constants.SegmentSize);

            if (rectangle.Intersects(headRectangle))
            {
                at = i;
                break;
            }
        }

        if (at >= 0)
        {
            _locations.RemoveAt(at);
            return true;
        }

        return false;
    }
}