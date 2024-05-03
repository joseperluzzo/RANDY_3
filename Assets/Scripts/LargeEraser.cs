using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LargeEraser : GridBrushBase
{
    public int radius = 1;  // Radius of the eraser

    public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
        if (brushTarget == null)
            return;

        Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
        if (tilemap != null)
        {
            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    Vector3Int location = new Vector3Int(position.x + x, position.y + y, position.z);
                    if (tilemap.GetTile(location) != null)  
                    {
                        tilemap.SetTile(location, null); 
                    }
                }
            }
        }
    }
}
