using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBRGlass : MonoBehaviour
{

    private Material m_Material;
    public GameObject prefab;

    void Start()
    {

        //金属度：0~0.5，平滑度0.5；
        for (int i = 0; i <= 5; i++)
        {
            Vector3 pos = new Vector3(i, 0, 0);
            GameObject go = GameObject.Instantiate(prefab, pos, Quaternion.identity);
            go.GetComponent<MeshRenderer>().material.SetFloat("_Metallic", i * 0.1f);
            go.GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", 0.5f);
        }

        //金属度：0~0.5，平滑度1；
        for (int i = 0; i <= 5; i++)
        {
            Vector3 pos = new Vector3(i, 0, -1);
            GameObject go = GameObject.Instantiate(prefab, pos, Quaternion.identity);
            go.GetComponent<MeshRenderer>().material.SetFloat("_Metallic", i * 0.1f);
            go.GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", 1);
        }

        //金属度：0，平滑度0.5~1；
        for (int i = 0; i <= 5; i++)
        {
            Vector3 pos = new Vector3(i, 0, -2);
            GameObject go = GameObject.Instantiate(prefab, pos, Quaternion.identity);
            go.GetComponent<MeshRenderer>().material.SetFloat("_Metallic", 0);
            go.GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", i * 0.1f);
        }

        //金属度0.5，平滑度0.5~1。
        for (int i = 0; i <= 5; i++)
        {
            Vector3 pos = new Vector3(i, 0, -3);
            GameObject go = GameObject.Instantiate(prefab, pos, Quaternion.identity);
            go.GetComponent<MeshRenderer>().material.SetFloat("_Metallic", 0.5f);
            go.GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", i * 0.1f);
        }
    }

}
