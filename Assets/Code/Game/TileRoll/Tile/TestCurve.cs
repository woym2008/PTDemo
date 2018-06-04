using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class TestCurve : MonoBehaviour
    {
        public Transform startpoint;
        public Transform point1;
        public Transform point2;
        public Transform endpoint;

        public MorphCurve RenderModel;
        Material m_SelfMaterial;

        public int pointnum = 30;

        private void Start()
        {
            
        }

        private void TestCreate()
        {
            List<Vector3> points = new List<Vector3>();
            float every_t = 1.0f / (float)pointnum;
            for (int i = 0; i < pointnum; ++i)
            {
                Vector3 pos = CalculateCubicBezierPoint(every_t * i, this.transform.position, point1.position,
                    point2.position, endpoint.position);
                points.Add(pos);
            }
            RenderModel.SetLength(10);
            RenderModel.SetWidth(0.1f);
            RenderModel.SetDepth(0.1f);

            RenderModel.CreateMesh(points.ToArray());

            m_SelfMaterial = RenderModel.GetMeshMaterial();
        }

        private void Update()
        {
            if(Input.GetKey(KeyCode.T))
            {

            }
            TestCreate();
        }
        Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * p0;
            p += 3 * uu * t * p1;
            p += 3 * u * tt * p2;
            p += ttt * p3;

            return p;
        }
    }
}
