using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathArrow : MonoBehaviour
{
    Tile tileOver;
    Tile tileDown;
    public LineRenderer lineRenderer;
    public PathFinding pathFinding;
    // Start is called before the first frame update
    void Start()
    {
        TileView.OnMouseDownTile += onMouseDownTileAction;
        TileView.OnMouseOverTile += onMouseOverAction;
    }

    void onMouseOverAction(Tile tile)
    {
        tileOver = tile;
        if(tileDown != null )
        {
            List<TileNode> path=pathFinding.findPath(tileDown.coordW,tileDown.coordH,tileOver.coordW,tileOver.coordH);
            if(path != null )
            {
                lineRenderer.positionCount = path.Count;
                for (int i = 0; i < path.Count; i++)
                {
                    TileNode node = path[i];
                    lineRenderer.SetPosition(i, MapViewAsset.getTilePosition(node.tile.coordW, node.tile.coordH, 1));
                }
            }
            
        }
    }

    void onMouseDownTileAction(Tile tile)
    {
        tileDown = tile;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
