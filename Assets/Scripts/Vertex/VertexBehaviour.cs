using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexBehaviour : MonoBehaviour
{
    public int vid = -1;
    // Start is called before the first frame update
    public MapManagerBehaviour mapManager = null;
    void Awake()
    {
        if (mapManager == null)
        {
            // fetch the MapManager from the parent's MapManagerBehaviour
            mapManager = transform.parent.GetComponent<MapManagerBehaviour>();
        }
        Debug.Assert(mapManager != null);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
