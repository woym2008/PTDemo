using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class MorphCurve : MonoBehaviour, IMorphObj
    {
        public Material m_material;
        //-------------------------------
        MeshFilter meshfilter;        
        MeshRenderer m_render;
        Mesh SelfMesh;
        //-------------------------------
        //BoxCollider m_Collider = null;
        MeshCollider m_MeshCollider = null;

        //MeshCollider m_MeshCollider;
        //-------------------------------
        private Vector3[] vertices;//顶点
        private Vector2[] uv;
        private int[] triangles;
        //-------------------------------
        public Vector3 UpDirect = new Vector3(0, 1, 0);
        public Vector3[] up_Directs;
        public float up_length = 1.0f;
        public float right_length = 1.0f;
        public float forward_length = 1.0f;

        //public float baselength = 4.0f;

        public int segment_right = 2;
        public int segment_up = 2;
        public int segment_forward = 2;

        float collidescale = 2.0f;

        //test
        //public List<GameObject> gameobjectlist = new List<GameObject>();

        //-------------------------------
        public void CreateMesh()
        {

        }
        //public void CreateMesh()
        //{
        //    if(SelfMesh == null)
        //    {
        //        SelfMesh = new Mesh();
        //    }

        //    m_Collider = this.gameObject.GetComponent<BoxCollider>();
        //    if (m_Collider == null)
        //    {
        //        m_Collider = this.gameObject.AddComponent<BoxCollider>();
        //    }

        //    BuildMesh();
        //}

        public void SetLength(float l)
        {
            up_length = l;
        }

        public void SetWidth(float l)
        {
            right_length = l;
        }

        public void SetDepth(float l)
        {
            forward_length = l;
        }

        public void CreateMesh(Vector3[] pos, Vector3[] normals)
        {
            up_Directs = new Vector3[normals.Length];
            for(int i=0;i<normals.Length; ++i)
            {
                up_Directs[i] = normals[i];
            }

            if (SelfMesh == null)
            {
                SelfMesh = new Mesh();
            }

            //m_Collider = this.gameObject.GetComponent<BoxCollider>();
            //if (m_Collider == null)
            //{
            //    m_Collider = this.gameObject.AddComponent<BoxCollider>();
            //}
            m_MeshCollider = this.gameObject.GetComponent<MeshCollider>();
            if (m_MeshCollider == null)
            {
                m_MeshCollider = this.gameObject.AddComponent<MeshCollider>();
            }

            //m_MeshCollider = this.gameObject.GetComponent<MeshCollider>();
            //if (m_MeshCollider == null)
            //{
            //    m_MeshCollider = this.gameObject.AddComponent<MeshCollider>();
            //}

            BuildMesh(pos);
        }
        //-------------------------------
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                //CreateMesh();
            }
        }
        //-------------------------------
        void BuildMesh(Vector3[] pos)
        {
            meshfilter = this.GetComponent<MeshFilter>();
            if(meshfilter == null)
            {
                meshfilter = this.gameObject.AddComponent<MeshFilter>();
            }

            m_render = this.gameObject.GetComponent<MeshRenderer>();
            if (m_render == null)
            {
                m_render = this.gameObject.AddComponent<MeshRenderer>();
            }

            SelfMesh.Clear();

            SetVertivesUV(pos);//生成顶点和uv信息
            SetTriangles();//生成索引

            SelfMesh.vertices = vertices;
            SelfMesh.uv = uv;
            SelfMesh.triangles = triangles;

            SelfMesh.RecalculateNormals();//重置法线

            SelfMesh.RecalculateBounds();   //重置范围

            meshfilter.mesh = SelfMesh;

            
            m_render.material = m_material;

            //m_Collider.size = new Vector3(
            //    right_length,
            //    up_length,
            //    forward_length) * collidescale;
            m_MeshCollider.sharedMesh = meshfilter.mesh;
            m_MeshCollider.convex = true;
        }
        private void SetVertivesUV(Vector3[] centerposs)
        {
            //test points
            //List<Vector3> points = new List<Vector3>();

            //以center为中心
            Vector3 center = centerposs[0];
            for(int i=0; i<centerposs.Length; ++i) {
                centerposs[i] = centerposs[i] - center;
            }

            //上方向是up
            //前方向需要求
            Vector3[] forwarddirs = new Vector3[centerposs.Length];
            Vector3[] rightdirs = new Vector3[centerposs.Length];

            for (int i = 0; i < forwarddirs.Length-1; ++i) {
                forwarddirs[i] = (centerposs[i+1] - centerposs[i]).normalized;
                //rightdirs[i] = Vector3.Cross(forwarddirs[i], UpDirect);
                rightdirs[i] = Vector3.Cross(forwarddirs[i], up_Directs[i]);
            }
            forwarddirs[forwarddirs.Length - 1] = forwarddirs[forwarddirs.Length - 2];
            rightdirs[rightdirs.Length - 1] = Vector3.Cross(forwarddirs[forwarddirs.Length - 1], up_Directs[up_Directs.Length - 1]);

            //顶点数
            int allpointnum = centerposs.Length * 8 + 8;

            //构造顶点
            vertices = new Vector3[allpointnum];
            uv = new Vector2[vertices.Length];

            float tinyforward = forward_length / (centerposs.Length - 1);
            float tinyup = up_length ;
            float tinyright = right_length ;
            int num = 0;

            segment_right = 2;
            segment_up = 2;
            segment_forward = centerposs.Length;

            //构建沿着传入线段方向的弯曲立方体
            //face1 front
            for (int y = 0; y < segment_forward; ++y)
            {                
                for (int x = 0; x < segment_right; ++x)
                {
                    //Vector3 pos = new Vector3(
                    //    centerposs[y].x + tinyleft * x,
                    //    centerposs[y].y + tinyup * 0.5f,
                    //    centerposs[y].z);

                    //Vector3 pos = centerposs[y] + rightdirs[y]*x*tinyright + UpDirect * tinyup * 0.5f;

                    Vector3 pos = centerposs[y] + rightdirs[y] * x * tinyright + up_Directs[y] * tinyup * 0.5f;

                    //左方向和上方向到中心去
                    pos = pos - rightdirs[y] * 0.5f * right_length;

                    vertices[num] = pos;                    

                    uv[num] = new Vector2((float)x / (segment_right - 1), 
                        (float)y / (segment_forward - 1));
                    num++;
                }
                
            }
            //face2 back
            for (int y = 0; y < segment_forward; ++y)
            {                
                for (int x = 0; x < segment_right; ++x)
                {
                    //Vector3 pos = new Vector3(
                    //    centerposs[y].x + tinywidth * x - width * 0.5f,
                    //    centerposs[y].y,
                    //    centerposs[y].z - depth * 0.5f);

                    Vector3 pos = centerposs[y] + rightdirs[y] * x * tinyright - up_Directs[y] * tinyup * 0.5f;

                    pos = pos - rightdirs[y] * 0.5f * right_length;

                    vertices[num] = pos;
                    uv[num] = new Vector2((float)x / (segment_right - 1), 
                        (float)y / (segment_forward - 1));
                    num++;
                }
            }

            for (int y = 0; y < segment_forward; ++y)
            {
                //left
                for (int z = 0; z < segment_up; ++z)
                {
                    //Vector3 pos = new Vector3(
                    //    centerposs[y].x - width * 0.5f,
                    //    centerposs[y].y,
                    //    centerposs[y].z - z * tinydepth + depth * 0.5f);

                    Vector3 pos = centerposs[y] + up_Directs[y] * z * tinyup + rightdirs[y] * tinyright *0.5f;
                    pos = pos - up_Directs[y] * 0.5f * up_length;

                    vertices[num] = pos;
                    uv[num] = new Vector2((float)z / (segment_up - 1), 
                        (float)y / (segment_forward - 1));
                    num++;
                }
            }

            for (int y = 0; y < segment_forward; ++y)
            {
                //right
                for (int z = 0; z < segment_up; ++z)
                {
                    //Vector3 pos = new Vector3(
                    //    centerposs[y].x + width * 0.5f,
                    //    centerposs[y].y,
                    //    centerposs[y].z - z * tinydepth + depth * 0.5f);

                    Vector3 pos = centerposs[y] + up_Directs[y] * z * tinyup - rightdirs[y] * tinyright * 0.5f;
                    pos = pos - up_Directs[y] * 0.5f * up_length;

                    vertices[num] = pos;
                    uv[num] = new Vector2((float)z / (segment_up - 1), 
                        (float)y / (segment_forward - 1));
                    num++;
                }
            }

            //start face
            for (int z = 0; z < segment_up; ++z)
            {
                for (int x = 0; x < segment_right; ++x)
                {
                    //Vector3 pos = new Vector3(
                    //    x * tinywidth - width * 0.5f,
                    //    0,
                    //    z * tinydepth - depth * 0.5f);

                    Vector3 pos = up_Directs[0] * z * tinyup + rightdirs[0] * tinyright *x;

                    pos = pos - up_Directs[0] * up_length * 0.5f - rightdirs[0] * right_length * 0.5f;

                    vertices[num] = pos;
                    uv[num] = new Vector2((float)x / (segment_right - 1), (float)z / (segment_up - 1));
                    num++;
                }
            }
            //top
            for (int z = 0; z < segment_up; ++z)
            {
                for (int x = 0; x < segment_right; ++x)
                {
                    //Vector3 pos = new Vector3(
                    //    x * tinywidth - width * 0.5f +
                    //    centerposs[centerposs.Length - 1].x,
                    //    centerposs[centerposs.Length -1].y, 
                    //    z * tinydepth - depth * 0.5f +
                    //    centerposs[centerposs.Length - 1].z);

                    Vector3 pos = centerposs[segment_forward-1] +
                        up_Directs[segment_forward - 1] * z * tinyup
                        + rightdirs[segment_forward-1] * tinyright * x;

                    pos = pos - up_Directs[segment_forward - 1] * up_length * 0.5f - rightdirs[segment_forward - 1] * right_length * 0.5f;

                    vertices[num] = pos;
                    uv[num] = new Vector2((float)x / (segment_right - 1), (float)z / (segment_up - 1));
                    num++;
                }
            }

            //test
            //for (int i = 0; i < centerposs.Length; ++i)
            //{
            //    GameObject newgameobj = new GameObject();
            //    newgameobj.transform.transform.parent = this.transform;
            //    newgameobj.transform.localPosition = centerposs[i];
            //}

        }

        private void SetTriangles()
        {
            //有问题 索引点计算不对
            //int numtriangles = (segment_x * segment_y);
            //triangles = new int[(
            //    (segment_x - 1) * (segment_y - 1) * 2 +
            //    (segment_y - 1) * (segment_z - 1) * 2 +
            //    (segment_x - 1) * (segment_z - 1) * 2)
            //    * 6];
            int numtriangles =
                (segment_forward - 1) * (segment_right - 1) * 12 +
                (segment_forward - 1) * (segment_up - 1) * 12 +
                (segment_up - 1) * (segment_right - 1) * 12;

            triangles = new int[numtriangles];

            int index = 0;//用来给三角形索引计数

            //Front Face
            for (int y = 0; y < segment_forward - 1; y++)
                for (int x = 0; x < segment_right - 1; x++)
                {
                    int line = segment_right;
                    int self = x + y * line;

                    triangles[index] = self;
                    triangles[index + 1] = self + 1;
                    triangles[index + 2] = self + line + 1;
                    triangles[index + 3] = self;
                    triangles[index + 4] = self + 1 + line;
                    triangles[index + 5] = self + line;
                    index += 6;
                }

            //Back Face
            for (int y = 0; y < segment_forward - 1; y++)
                for (int x = 0; x < segment_right - 1; x++)
                {
                    int line = segment_right;
                    int self = x + segment_right * segment_forward + y * line;

                    triangles[index] = self;
                    triangles[index + 1] = self + line + 1;
                    triangles[index + 2] = self + 1;
                    triangles[index + 3] = self;
                    triangles[index + 4] = self + line;
                    triangles[index + 5] = self + line + 1;
                    index += 6;
                }

            //Left Face
            for (int y = 0; y < segment_forward - 1; y++)
                for (int z = 0; z < segment_up - 1; z++)
                {
                    int line = segment_up;
                    int self = z + segment_up * segment_forward * 2 + line * y;

                    triangles[index] = self;
                    triangles[index + 1] = self + line + 1;
                    triangles[index + 2] = self + 1;
                    triangles[index + 3] = self;
                    triangles[index + 4] = self + line;
                    triangles[index + 5] = self + line + 1;
                    //triangles[index] = self + 1;
                    //triangles[index + 1] = self + line;
                    //triangles[index + 2] = self;
                    //triangles[index + 3] = self + 1;
                    //triangles[index + 4] = self + 1 + line;
                    //triangles[index + 5] = self + line;
                    index += 6;
                }

            //Right Face
            for (int y = 0; y < segment_forward - 1; y++)
                for (int z = 0; z < segment_up - 1; z++)
                {
                    int line = segment_up;
                    int self = z + segment_up * segment_forward * 2 + segment_forward * segment_right + line * y;

                    //triangles[index] = self;
                    //triangles[index + 1] = self + line + 1;
                    //triangles[index + 2] = self + 1;
                    //triangles[index + 3] = self;
                    //triangles[index + 4] = self + line;
                    //triangles[index + 5] = self + line + 1;
                    triangles[index] = self;
                    triangles[index + 1] = self + 1;
                    triangles[index + 2] = self + line + 1;
                    triangles[index + 3] = self;
                    triangles[index + 4] = self + 1 + line;
                    triangles[index + 5] = self + line;
                    index += 6;
                }

            //Bottom Face
            for (int z = 0; z < segment_up - 1; z++)
                for (int x = 0; x < segment_right - 1; x++)
                {
                    int line = segment_right;
                    int self = x + segment_right * segment_forward * 2 +
                        segment_forward * segment_up * 2 + line * z;

                    //triangles[index] = self + line;
                    //triangles[index + 1] = self + 1;
                    //triangles[index + 2] = self + line + 1;
                    //triangles[index + 3] = self + line;
                    //triangles[index + 4] = self;
                    //triangles[index + 5] = self + 1;

                    triangles[index] = self;
                    triangles[index + 1] = self + 1;
                    triangles[index + 2] = self + line + 1;
                    triangles[index + 3] = self;
                    triangles[index + 4] = self + 1 + line;
                    triangles[index + 5] = self + line;
                    index += 6;
                }

            //Top Face
            for (int z = 0; z < segment_up - 1; z++)
                for (int x = 0; x < segment_right - 1; x++)
                {
                    int line = segment_right;
                    int self = x + segment_right * segment_forward * 2 + segment_forward * segment_up * 2
                        + segment_right * segment_up + line * z;

                    triangles[index] = self;
                    triangles[index + 1] = self + line + 1;
                    triangles[index + 2] = self + 1;
                    triangles[index + 3] = self;
                    triangles[index + 4] = self + line;
                    triangles[index + 5] = self + line + 1;

                    index += 6;
                }
        }

        public void SetPosition(Vector3 worldpos)
        {
            //this.transform.position = new Vector3(
            //    this.transform.position.x + ,
            //    this.transform.position.y,
            //    this.transform.position.z);
        }

        public void SetMaterial(Material mat)
        {
            this.m_material = mat;
        }

        public Material GetMeshMaterial()
        {
            if(m_render != null)
            {
                return m_render.material;
            }

            return null;
        }
    }
}
