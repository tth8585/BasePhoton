using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetWorkPrefabsController
{
    public GameObject prefab;
    public string path;

    public NetWorkPrefabsController(GameObject obj,string path)
    {
        prefab = obj;
        this.path = path;
    }
}
