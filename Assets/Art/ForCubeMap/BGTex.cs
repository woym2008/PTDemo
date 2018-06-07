using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGTex : MonoBehaviour {

    public Material m_bgmat;

    public float speed = 0.01f;
    Vector2 offset = new Vector2(0.0f, 0.0f);

	// Use this for initialization
	void Start () {
        m_bgmat = this.gameObject.GetComponent<MeshRenderer>().material;

    }
	
	// Update is called once per frame
	void Update () {
        offset = new Vector2(offset.x + Time.deltaTime * speed, offset.y);
        m_bgmat.SetTextureOffset("_MainTex", offset);
    }
}
