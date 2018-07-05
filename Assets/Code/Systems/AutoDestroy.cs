/* ========================================================
/* 临时加的，以后删掉
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

    public float lifeTime = 1.0f;

	// Use this for initialization
	void Start () {
        Invoke("selfDestroy", lifeTime);
	}
	
    void selfDestroy()
    {
        GameObject.Destroy(base.gameObject);
        
    }
	
}
