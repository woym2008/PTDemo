using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Demo;

namespace Art
{
    public class MorphGlass : MonoBehaviour
    {
        public MorphCube RenderModel;

        // Use this for initialization
        void Start()
        {
            MeshRenderer mr = this.gameObject.GetComponent<MeshRenderer>();
            RenderModel = this.gameObject.GetComponent<MorphCube>();

            RenderModel.SetLength(0.1f);
            RenderModel.SetWidth(0.1f);
            RenderModel.SetDepth(0.35f);
            RenderModel.CreateMesh();
            RenderModel.SetMaterial(mr.material);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

