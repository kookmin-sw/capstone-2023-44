using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExtendPRS
{

    public Transform obj;

    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

    public ExtendPRS()
    {
        this.pos = Vector3.zero;
        this.rot = Quaternion.identity;
        this.scale = Vector3.zero;
    }

    public ExtendPRS(Vector3 pos, Quaternion rot, Vector3 scale)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
    }
    public ExtendPRS(Transform obj, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        this.obj = obj;
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
    }

}

public class Utill
{
    public static void ToggleCollider(GameObject obj)
    {
        if(obj.GetComponent<BoxCollider>().enabled)
        {
            obj.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            obj.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
