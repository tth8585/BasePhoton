using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MasterManager : ScriptableObjectSingleton<MasterManager>
{
    List<NetWorkPrefabsController> _netWorkPrefabsControllers = new List<NetWorkPrefabsController>();
    public static void NetworkInstantiate(GameObject obj,Vector3 pos,Quaternion qua)
    {
        //foreach(NetWorkPrefabsController netWorkPrefabsController in)
    }
}
