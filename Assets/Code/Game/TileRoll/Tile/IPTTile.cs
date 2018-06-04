using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IPTTile
{
    float getStartTime();

    float getProcess();

    void setPosition(Vector3 pos);

    void setRotation(Quaternion quat);

    void setScale(Vector3 scale);
}
