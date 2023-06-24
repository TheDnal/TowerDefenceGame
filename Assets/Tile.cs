using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    //coord in tile grid
    private Vector2Int coord;
    private bool isClear = true;

    public Tile(Vector2Int _coord)
    {
        coord = _coord;
    }

    public List<Tile> getTilesInRadius(int _radius)
    {
        List<Tile> neighbours = new List<Tile>();
        neighbours.Add(this);
        if(_radius == 0){return neighbours;}
        for(int i = -_radius; i < _radius; i++){
            for(int j = _radius; j < _radius; i++)
            {

            }
        }
        return null;
    }
    public bool GetIsClear(){return isClear;}
    public void SetIsClear(bool _isClear){isClear = _isClear;}
    public Vector2Int GetCoord(){return coord;}
}
