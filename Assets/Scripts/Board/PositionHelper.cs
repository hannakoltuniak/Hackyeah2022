using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public static class PositionHelper
    {

        public static Boolean IsFirstAdjcaentToSecond(Int32 firstX, Int32 firstY, Int32 secondX, Int32 secondY)
        {
            return (firstX == secondX - 1 && firstY == secondY)//left
                || (firstX == secondX + 1 && firstY == secondY)//right
                || (firstX == secondX && firstY == secondY + 1)//top
                || (firstX == secondX && firstY == secondY - 1);//down
        }

        public static List<Vector2Int> ExtractCoordinatesFromGroupInfo((Vector2 sourceTile, Vector2 groupVector) group)
        {
            List<Vector2Int> result = new List<Vector2Int>();

            if (group.groupVector.y > 0)//up
                for (Int32 y = (Int32)group.sourceTile.y; y < (Int32)group.sourceTile.y + (Int32)group.groupVector.y; y++)
                    result.Add(new Vector2Int((Int32)group.sourceTile.x, y));
            else if (group.groupVector.x > 0)//right
                for (Int32 x = (Int32)group.sourceTile.x; x < (Int32)group.sourceTile.x + (Int32)group.groupVector.x; x++)
                    result.Add(new Vector2Int(x, (Int32)group.sourceTile.y));
            else if (group.groupVector.x < 0)//left
                for (Int32 x = (Int32)group.sourceTile.x; x > (Int32)group.sourceTile.x + (Int32)group.groupVector.x; x--)
                    result.Add(new Vector2Int(x, (Int32)group.sourceTile.y));
            else if (group.groupVector.y < 0)//down
                for (Int32 y = (Int32)group.sourceTile.y; y > (Int32)group.sourceTile.y + (Int32)group.groupVector.y; y--)
                    result.Add(new Vector2Int((Int32)group.sourceTile.x, y));

            return result;
        }

        public static Vector2 CheckIfMakesGroup(Slot toCheck, List<Vector2Int> toIgnore)
        {
            {
                Int32 toUpInGroup = 1;
                while (!toIgnore.Any(ti => ti.x == toCheck.X && ti.y == (toCheck.Y + toUpInGroup)) && BoardController.Instance.GetSlotAt(toCheck.X, toCheck.Y + toUpInGroup)?.Tile.TileKind == toCheck.Tile.TileKind)
                    toUpInGroup++;

                if (toUpInGroup >= 3)//
                    return new Vector2(0, toUpInGroup);
            }


            {
                Int32 toRightInGroup = 1;
                while (!toIgnore.Any(ti => ti.x == (toCheck.X + toRightInGroup) && ti.y == toCheck.Y) && BoardController.Instance.GetSlotAt(toCheck.X + toRightInGroup, toCheck.Y)?.Tile.TileKind == toCheck.Tile.TileKind)
                    toRightInGroup++;

                if (toRightInGroup >= 3)
                    return new Vector2(toRightInGroup, 0);
            }

            {
                Int32 toLeftInGroup = 1;
                while (!toIgnore.Any(ti => ti.x == (toCheck.X - toLeftInGroup) && ti.y == toCheck.Y) && BoardController.Instance.GetSlotAt(toCheck.X - toLeftInGroup, toCheck.Y)?.Tile.TileKind == toCheck.Tile.TileKind)
                    toLeftInGroup++;

                if (toLeftInGroup >= 3)
                    return new Vector2(-toLeftInGroup, 0);
            }

            {
                Int32 toDownInGroup = 1;
                while (!toIgnore.Any(ti => ti.x == toCheck.X && ti.y == (toCheck.Y - toDownInGroup)) && BoardController.Instance.GetSlotAt(toCheck.X, toCheck.Y - toDownInGroup)?.Tile.TileKind == toCheck.Tile.TileKind)
                    toDownInGroup++;

                if (toDownInGroup >= 3)
                    return new Vector2(0, -toDownInGroup);
            }

            return Vector3.zero;

        }

        public static (Vector2Int swapThis, Vector2Int withThis) GetMoveThatMakesGroup(Slot toCheck)
        {
            {
                Int32 toUpInGroup = 1;
                while (BoardController.Instance.GetSlotAt(toCheck.X, toCheck.Y + toUpInGroup)?.Tile.TileKind == toCheck.Tile.TileKind)
                    toUpInGroup++;

                if (toUpInGroup == 2)
                {
                    var tilePosToSwapWithToGetGroup = GetNeighboringTilePositionExcludingDirection(Vector2Int.down, new Vector2Int(toCheck.X, toCheck.Y) + new Vector2Int(0, toUpInGroup), toCheck.Tile.TileKind);
                    if (tilePosToSwapWithToGetGroup.swapThis != Vector2.zero && tilePosToSwapWithToGetGroup.withThis != Vector2.zero)//neighboring with one from the group of 2 and another
                    {
                        return (tilePosToSwapWithToGetGroup.swapThis, tilePosToSwapWithToGetGroup.withThis);
                    }
                }
                //return new Vector2(0, toUpInGroup);
            }


            {
                Int32 toRightInGroup = 1;
                while (BoardController.Instance.GetSlotAt(toCheck.X + toRightInGroup, toCheck.Y)?.Tile.TileKind == toCheck.Tile.TileKind)
                    toRightInGroup++;

                if (toRightInGroup == 2)
                {
                    var tilePosToSwapWithToGetGroup = GetNeighboringTilePositionExcludingDirection(Vector2Int.left, new Vector2Int(toCheck.X, toCheck.Y) + new Vector2Int(toRightInGroup, 0), toCheck.Tile.TileKind);
                    if (tilePosToSwapWithToGetGroup.swapThis != Vector2.zero && tilePosToSwapWithToGetGroup.withThis != Vector2.zero)//neighboring with one from the group of 2 and another
                    {
                        return (tilePosToSwapWithToGetGroup.swapThis, tilePosToSwapWithToGetGroup.withThis);
                    }
                }
            }

            {
                Int32 toLeftInGroup = 1;
                while (BoardController.Instance.GetSlotAt(toCheck.X - toLeftInGroup, toCheck.Y)?.Tile.TileKind == toCheck.Tile.TileKind)
                    toLeftInGroup++;

                if (toLeftInGroup == 2)
                {
                    var tilePosToSwapWithToGetGroup = GetNeighboringTilePositionExcludingDirection(Vector2Int.right, new Vector2Int(toCheck.X, toCheck.Y) + new Vector2Int(-toLeftInGroup, 0), toCheck.Tile.TileKind);
                    if (tilePosToSwapWithToGetGroup.swapThis != Vector2.zero && tilePosToSwapWithToGetGroup.withThis != Vector2.zero)//neighboring with one from the group of 2 and another
                    {
                        return (tilePosToSwapWithToGetGroup.swapThis, tilePosToSwapWithToGetGroup.withThis);
                    }
                }
            }

            {
                Int32 toDownInGroup = 1;
                while (BoardController.Instance.GetSlotAt(toCheck.X, toCheck.Y - toDownInGroup)?.Tile.TileKind == toCheck.Tile.TileKind)
                    toDownInGroup++;

                if (toDownInGroup == 2)
                {
                    var tilePosToSwapWithToGetGroup = GetNeighboringTilePositionExcludingDirection(Vector2Int.up, new Vector2Int(toCheck.X, toCheck.Y) + new Vector2Int(0, -toDownInGroup), toCheck.Tile.TileKind);
                    if (tilePosToSwapWithToGetGroup.swapThis != Vector2.zero && tilePosToSwapWithToGetGroup.withThis != Vector2.zero)//neighboring with one from the group of 2 and another
                    {
                        return (tilePosToSwapWithToGetGroup.swapThis, tilePosToSwapWithToGetGroup.withThis);
                    }
                }
            }

            return (Vector2Int.zero, Vector2Int.zero);

        }

        public static (Vector2Int swapThis, Vector2Int withThis) GetNeighboringTilePositionExcludingDirection(Vector2Int ignoredDirection, Vector2Int toCheck, Int32 kindToCheckIfNeighboring)
        {
            List<Vector2Int> upDownLeftRightOffests = new List<Vector2Int>
            {
               new Vector2Int(0,1), new Vector2Int(0,-1), new Vector2Int(-1,0), new Vector2Int(1,0)//offsets to get positions in direction
            };

            upDownLeftRightOffests.Remove(ignoredDirection);

            for (Int32 i = 0; i < 3; i++)
            {
                Slot neighbor = BoardController.Instance.GetSlotAt(toCheck.x + upDownLeftRightOffests[i].x, toCheck.y + upDownLeftRightOffests[i].y);
                if (neighbor != null && neighbor.Tile.TileKind == kindToCheckIfNeighboring)
                {
                    var result = toCheck + upDownLeftRightOffests[i];
                    return (toCheck, result);
                }
            }

            return (Vector2Int.zero, Vector2Int.zero);
        }

        public static Vector3 RandomPointOnCircleEdge(Single radius)
        {
            var vector2 = Random.insideUnitCircle.normalized * radius;
            return new Vector3(vector2.x, 0, vector2.y);
        }

        public static bool LineLineIntersection(out Vector3 intersection,
                                                    Vector3 linePoint1, Vector3 lineDirection1,
                                                    Vector3 linePoint2, Vector3 lineDirection2)
        {
            Vector3 lineVec3 = linePoint2 - linePoint1;
            Vector3 crossVec1and2 = Vector3.Cross(lineDirection1, lineDirection2);
            Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineDirection2);
            float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

            //is coplanar, and not parallel
            if (Mathf.Abs(planarFactor) < 0.0001f
                    && crossVec1and2.sqrMagnitude > 0.0001f)
            {
                float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
                intersection = linePoint1 + (lineDirection1 * s);
                return true;
            }
            else
            {
                intersection = Vector3.zero;
                return false;
            }
        }

    }
}
