using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchCloud : MonoBehaviour {

    public float speed = 1.0f;
    Vector3 m_direct;

    float MoveTime = 1.0f;
    float curMoveTime = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(curMoveTime > 0)
        {
            this.transform.position += m_direct * speed * Time.deltaTime;

            curMoveTime -= Time.deltaTime;
        }
	}

    public void MoveCloud(Camera main)
    {
        if(main != null)
        {
            Vector3 movedir = Vector3.Cross(main.transform.forward, new Vector3(0, 1, 0));
            if(this.transform.position.x > main.transform.position.x)
            {
                m_direct = -movedir;
            }
            else
            {
                m_direct = movedir;
            }

            curMoveTime = MoveTime;
        }
    }
}
