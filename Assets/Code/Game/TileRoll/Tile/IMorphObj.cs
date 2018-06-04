using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public interface IMorphObj
    {
        void SetLength(float l);

        void SetPosition(Vector3 worldpos);

        void CreateMesh();

        void SetMaterial(Material mat);
    }
}
