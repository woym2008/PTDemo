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
        BoxCollider m_Collider = null;

        //MeshCollider m_MeshCollider;
        //-------------------------------
        private Vector3[] vertices;//顶点
        private Vector2[] uv;
        private int[] triangles;
        //-------------------------------
        public Vector3 ExtendDirect = new Vector3(0, 1, 0);
        public float length = 1.0f;
        public float width = 1.0f;
        public float depth = 1.0f;

        //public float baselength = 4.0f;

        public int segment_x = 2;
        public int segment_y = 2;
        public int segment_z = 2;

        //-------------------------------
        public void CreateMesh()
        {

        }

        public void SetLength(float l)
        {
            length = l;
        }

        public void SetWidth(float l)
        {
            width = l;
        }

        public void SetDepth(float l)
        {
            depth = l;
        }

        public void CreateMesh(Vector3[] pos)
        {
            if (SelfMesh == null)
            {
                SelfMesh = new Mesh();
            }

            m_Collider = this.gameObject.GetComponent<BoxCollider>();
            if (m_Collider == null)
            {
                m_Collider = this.gameObject.AddComponent<BoxCollider>();
            }

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
            //Matrix4x4 mt = Matrix4x4.Rotate(Quaternion.Euler(90, 0, 0));
            //for (int i = 0; i < pos.Length; ++i)
            //{
            //    pos[i] = mt * pos[i];
            //}
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

            m_Collider.center = new Vector3(width * 0.5f * ExtendDirect.x,
                length * 0.5f * ExtendDirect.y,
                depth * 0.5f * ExtendDirect.z);
            //m_Collider.center = new Vector3(0,length*0.5f,0);
            //m_Collider.size = m_render.bounds.size;

            //temp
            //m_Collider.size = new Vector3(
            //    m_render.bounds.size.x,
            //    m_render.bounds.size.y, 
            //    m_render.bounds.size.z);
            //m_Collider.size = new Vector3(
            //    width,
            //    length,
            //    depth);
            //end
            //m_Collider.bounds.

            //m_MeshCollider.sharedMesh = this.SelfMesh;
        }
        private void SetVertivesUV(Vector3[] centerposs)
        {
            //中间曲线是Y方向上的 算出XZ
            int allpointnum = centerposs.Length * 8 + 8;
            //int allpointnum = 2 * segment_x * segment_y + 2 * segment_y * segment_z + 2 * segment_x * segment_z;
            //vertices = new Vector3[24 + (allpointnum - 8) * 2];
            vertices = new Vector3[allpointnum];
            //vertices = new Vector3[segment_x * segment_y * segment_z * 3];
            uv = new Vector2[vertices.Length];

            float tinyheight = length / (centerposs.Length - 1);
            float tinywidth = width ;
            float tinydepth = depth ;
            int num = 0;

            segment_x = 2;
            segment_z = 2;
            segment_y = centerposs.Length;

            for (int y = 0; y < segment_y; ++y)
            {
                //front
                for (int x = 0; x < segment_x; ++x)
                {
                    Vector3 pos = new Vector3(
                        centerposs[y].x + tinywidth * x - width * 0.5f,
                        centerposs[y].y,
                        centerposs[y].z + depth * 0.5f)
                        
                        - centerposs[0];

                    vertices[num] = pos;                    

                    uv[num] = new Vector2((float)x / (segment_x - 1), (float)y / (centerposs.Length - 1));
                    num++;
                }
                
            }
            for (int y = 0; y < segment_y; ++y)
            {
                //back
                for (int x = 0; x < segment_x; ++x)
                {
                    Vector3 pos = new Vector3(
                        centerposs[y].x + tinywidth * x - width * 0.5f,
                        centerposs[y].y,
                        centerposs[y].z - depth * 0.5f)

                        - centerposs[0];

                    vertices[num] = pos;
                    uv[num] = new Vector2((float)x / (segment_x - 1), (float)y / (centerposs.Length - 1));
                    num++;
                }
            }

            for (int y = 0; y < segment_y; ++y)
            {
                //left
                for (int z = 0; z < segment_z; ++z)
                {
                    Vector3 pos = new Vector3(
                        centerposs[y].x - width * 0.5f,
                        centerposs[y].y,
                        centerposs[y].z - z * tinydepth + depth * 0.5f)

                        - centerposs[0];

                    vertices[num] = pos;
                    uv[num] = new Vector2((float)z / (segment_z - 1), (float)y / (centerposs.Length - 1));
                    num++;
                }
            }

            for (int y = 0; y < segment_y; ++y)
            {
                //right
                for (int z = 0; z < segment_z; ++z)
                {
                    Vector3 pos = new Vector3(
                        centerposs[y].x + width * 0.5f,
                        centerposs[y].y,
                        centerposs[y].z - z * tinydepth + depth * 0.5f)

                        - centerposs[0];

                    vertices[num] = pos;
                    uv[num] = new Vector2((float)z / (segment_z - 1), (float)y / (centerposs.Length - 1));
                    num++;
                }
            }

            //bottom
            for (int z = 0; z < segment_z; ++z)
            {
                for (int x = 0; x < segment_x; ++x)
                {
                    Vector3 pos = new Vector3(
                        x * tinywidth - width * 0.5f,
                        0,
                        z * tinydepth - depth * 0.5f);
                    vertices[num] = pos;
                    uv[num] = new Vector2((float)x / (segment_x - 1), (float)z / (segment_z - 1));
                    num++;
                }
            }
            //top
            for (int z = 0; z < segment_z; ++z)
            {
                for (int x = 0; x < segment_x; ++x)
                {
                    Vector3 pos = new Vector3(
                        x * tinywidth - width * 0.5f +
                        centerposs[centerposs.Length - 1].x,
                        centerposs[centerposs.Length -1].y, 
                        z * tinydepth - depth * 0.5f +
                        centerposs[centerposs.Length - 1].z)

                        - centerposs[0];
                    vertices[num] = pos;
                    uv[num] = new Vector2((float)x / (segment_x - 1), (float)z / (segment_z - 1));
                    num++;
                }
            }
        }

        private void SetTriangles()
        {
            //有问题 索引点计算不对
            int numtriangles = (segment_x * segment_y);
            triangles = new int[(
                (segment_x - 1) * (segment_y - 1) * 2 +
                (segment_y - 1) * (segment_z - 1) * 2 +
                (segment_x - 1) * (segment_z - 1) * 2)
                * 6];
            int index = 0;//用来给三角形索引计数

            //Front Face
            for (int y = 0; y < segment_y - 1; y++)
                for (int x = 0; x < segment_x - 1; x++)
                {
                    int line = segment_x;
                    int self = x + y * line;

                    //triangles[index] = self;
                    //triangles[index + 1] = self + line + 1;
                    //triangles[index + 2] = self + 1;
                    //triangles[index + 3] = self;
                    //triangles[index + 4] = self + line;
                    //triangles[index + 5] = self + line + 1;

                    triangles[index] = self + 1;
                    triangles[index + 1] = self + line;
                    triangles[index + 2] = self;
                    triangles[index + 3] = self + 1;
                    triangles[index + 4] = self + 1 + line;
                    triangles[index + 5] = self + line;
                    index += 6;
                }

            //Back Face
            for (int y = 0; y < segment_y - 1; y++)
                for (int x = 0; x < segment_x - 1; x++)
                {
                    int line = segment_x;
                    int self = x + segment_x * segment_y + y * line;

                    //triangles[index] = self + 1;
                    //triangles[index + 1] = self + line;
                    //triangles[index + 2] = self;
                    //triangles[index + 3] = self + 1;
                    //triangles[index + 4] = self + 1 + line;
                    //triangles[index + 5] = self + line;

                    triangles[index] = self;
                    triangles[index + 1] = self + line + 1;
                    triangles[index + 2] = self + 1;
                    triangles[index + 3] = self;
                    triangles[index + 4] = self + line;
                    triangles[index + 5] = self + line + 1;
                    index += 6;
                }

            //Left Face
            for (int y = 0; y < segment_y - 1; y++)
                for (int z = 0; z < segment_x - 1; z++)
                {
                    int line = segment_z;
                    int self = z + segment_x * segment_y * 2 + line * y;

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
            for (int y = 0; y < segment_y - 1; y++)
                for (int z = 0; z < segment_x - 1; z++)
                {
                    int line = segment_z;
                    int self = z + segment_x * segment_y * 2 + segment_y * segment_z + line * y;

                    //triangles[index] = self;
                    //triangles[index + 1] = self + line + 1;
                    //triangles[index + 2] = self + 1;
                    //triangles[index + 3] = self;
                    //triangles[index + 4] = self + line;
                    //triangles[index + 5] = self + line + 1;
                    triangles[index] = self + 1;
                    triangles[index + 1] = self + line;
                    triangles[index + 2] = self;
                    triangles[index + 3] = self + 1;
                    triangles[index + 4] = self + 1 + line;
                    triangles[index + 5] = self + line;
                    index += 6;
                }

            //Bottom Face
            for (int z = 0; z < segment_z - 1; z++)
                for (int x = 0; x < segment_x - 1; x++)
                {
                    int line = segment_x;
                    int self = x + segment_x * segment_y * 2 + segment_y * segment_z * 2 + line * z;

                    //triangles[index] = self + line;
                    //triangles[index + 1] = self + 1;
                    //triangles[index + 2] = self + line + 1;
                    //triangles[index + 3] = self + line;
                    //triangles[index + 4] = self;
                    //triangles[index + 5] = self + 1;

                    triangles[index] = self + 1;
                    triangles[index + 1] = self + line;
                    triangles[index + 2] = self;
                    triangles[index + 3] = self + 1;
                    triangles[index + 4] = self + line + 1;
                    triangles[index + 5] = self + line;
                    index += 6;
                }

            //Top Face
            for (int z = 0; z < segment_z - 1; z++)
                for (int x = 0; x < segment_x - 1; x++)
                {
                    int line = segment_x;
                    int self = x + segment_x * segment_y * 2 + segment_y * segment_z * 2
                        + segment_x * segment_z + line * z;

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
