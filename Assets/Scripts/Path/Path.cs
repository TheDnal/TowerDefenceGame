using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    /*
        This class controls the path that the enemies will traverse
    */
    [SerializeField] private List<Transform> pathNodes = new List<Transform>();
    public bool showPath = false;
    public static Path instance;
    void Awake()
    {
        if(instance != null)
        {
            if(instance != this)
            {
                Destroy(this);
            }
        }
        instance = this;
    }
    public Transform GetPointInPath(int _index)
    {
        if(_index < 0){return null;}
        if(_index >= pathNodes.Count){return null;}
        return pathNodes[_index];
    }
    public int GetPathLength(){return pathNodes.Count;}
    void OnDrawGizmos()
    {
        if(pathNodes.Count == 0 || !showPath){return;}
        for(int i = 0; i < pathNodes.Count; i++)
        {
            if(i == pathNodes.Count - 1){continue;}
            Vector3 pos = pathNodes[i].position;
            Vector3 pos2 = pathNodes[i + 1].position;
            Gizmos.color = Color.Lerp(Color.red,Color.green, (float)i / pathNodes.Count);
            Gizmos.DrawLine(pos, pos2);
        }
    }
}
