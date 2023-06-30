using System.Drawing;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace aDevGame.sceneResources.scripts
{
    public static class MapTool
    {
        public static Vector2Int[] RandomWalk(Size tilemapSize, Vector2Int[] startPoints, int maxIterations, System.Random random)
        {
            var blockResult = new Vector2Int[startPoints.Length * maxIterations];
            var blockIndex = 0;
            foreach (Vector2Int startPoint in startPoints)
            {
                var currentX = startPoint.x;
                var currentY = startPoint.y;
                var iterationsCounter = 0;
                while (iterationsCounter < maxIterations)
                {
                    blockResult[blockIndex] = new Vector2Int(currentX, currentY);
                    iterationsCounter++;
                    blockIndex++;
                    var direction = random.Next(4);
                    switch (direction)
                    {
                        case 0:
                            if ((currentY + 1) < tilemapSize.Height)
                            {
                                currentY++;
                            }

                            break;
                        case 1:
                            if ((currentY - 1) >= 0)
                            {
                                currentY--;
                            }

                            break;
                        case 2:
                            if ((currentX - 1) >= 0)
                            {
                                currentX--;
                            }

                            break;
                        case 3:
                            if ((currentX + 1) < tilemapSize.Width)
                            {
                                currentX++;
                            }

                            break;
                    }
                }
            }

            return blockResult;
        }

        public static void DrawTilemap(Size tilemapSize, Tilemap tilemap, TileBase tile, Vector2Int[] drawPoints)
        {
            foreach (var drawPoint in drawPoints)
            {
                var tilePosition = new Vector3Int(drawPoint.x, drawPoint.y, 0);
                tilePosition += MMTilemapGridRenderer.ComputeOffset(tilemapSize.Width, tilemapSize.Height);
                tilemap.SetTile(tilePosition, tile);
            }
        }
    }
}