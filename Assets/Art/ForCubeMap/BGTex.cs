using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGTex : MonoBehaviour {

    private Material m_bgmat;

    public float speed = 0.01f;
    Vector2 offset = new Vector2(0.0f, 0.0f);

    public float alphaspeed = 0.1f;
    float curalphaspeed = 0.0f;
    float m_curalpha;

    public Color m_color;

	// Use this for initialization
	void Start () {
        m_bgmat = this.gameObject.GetComponent<MeshRenderer>().material;

        curalphaspeed = alphaspeed;
    }
	
	// Update is called once per frame
	void Update () {
        offset = new Vector2(offset.x + Time.deltaTime * speed, offset.y);
        m_bgmat.SetTextureOffset("_MainTex", offset);

        if(curalphaspeed > 0.0f)
        {
            //curalphaspeed += 0.1f * Time.deltaTime;
            if(m_curalpha > 0.5f)
            {
                curalphaspeed = -alphaspeed;
            }
        }
        else if (curalphaspeed < 0.0f)
        {
            if (m_curalpha <= 0.0f)
            {
                curalphaspeed = alphaspeed;
            }
        }

        m_curalpha += Time.deltaTime * curalphaspeed;

        Color nc = new Color(m_color.r, m_color.g, m_color.b,
            m_curalpha);
        m_bgmat.SetColor("_TintColor", nc);        
    }
}
