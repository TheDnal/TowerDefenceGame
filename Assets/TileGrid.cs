using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    Tile[,] grid;
    public Vector2Int gridDimensions;
    public int tileDiameter = 1;
    public static TileGrid instance;
    public bool showGizmos = false;
    public void Awake()
    {
        if(instance != null)
        {
            if(instance != this)
            {
                Destroy(this);
            }
        }
        instance = this;
        grid = new Tile[gridDimensions.x,gridDimensions.y];
        GenerateGrid();
    }
    void Start()
    {
        UpdateObstructions();
    }
    private void GenerateGrid()
    {
        for(int i = 0; i < gridDimensions.x; i++){
            for(int j = 0; j < gridDimensions.y; j++)
            {
                Vector2Int coord = new Vector2Int(i,j);
                Tile newTile = new Tile(coord);
                grid[i,j] = newTile;
            }
        }
    }
    public Tile GetNearestTile(Vector3 pos)
    {
        Vector3 relativePos = pos - transform.position;
        Vector2Int realPos = new Vector2Int(Mathf.RoundToInt(relativePos.x),Mathf.RoundToInt(relativePos.y));
        if(realPos.x < 0 || realPos.y < 0){return null;}
        if(realPos.y >= gridDimensions.y || realPos.x >= gridDimensions.x){return null;}
        return grid[realPos.x,realPos.y];
    }
    private void UpdateObstructions()
    {
        foreach(Tile tile in grid)
        {
            Vector2 pos = new Vector2(transform.position.x + tile.GetCoord().x,transform.position.y + tile.GetCoord().y);
            Vector2 startCorner,endCorner;
            startCorner = pos - (Vector2.one * 0.25f);
            endCorner = pos + (Vector2.one * 0.25f);
            if(Physics2D.OverlapArea(startCorner,endCorner, LayerMask.GetMask("Obstruction")))
            {
                tile.SetIsClear(false);
            }
        }
    }
    public void ObstructPath(List<Transform> _path)
    {
        Vector3 startPos, endPos;
        for(int i =0; i < _path.Count - 1; i++)
        {
            startPos = _path[i].position;
            endPos = _path[i + 1].position;
            float distance = Vector3.Distance(startPos,endPos);
            int roundedDistance = Mathf.RoundToInt(distance);
            Vector3 direction = endPos - startPos;
            direction.Normalize();
            for(int j = 0; j < roundedDistance; j++)
            {
                Vector3 pos = startPos + direction * j;
                Tile pathTile = GetNearestTile(pos);
                if(pathTile == null){continue;}
                pathTile.SetIsClear(false);
            }
        }
    }
    void OnDrawGizmos()
    {
        if(grid == null){return;}
        if(!showGizmos){return;}
        foreach(Tile tile in grid)
        {
            Vector3 pos = new Vector3(tile.GetCoord().x,tile.GetCoord().y,0);
            Gizmos.color = tile.GetIsClear() ? Color.white : Color.red;
            Gizmos.DrawCube(transform.position + pos, Vector3.one * 0.1f);
        }
    }
}
