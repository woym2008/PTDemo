﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IPTTile
{
    float getStartTime();

    float getProcess();

    void setProcess(float process);

    float getStartProcess();

    float getEndProcess();

    float getPositionProgress();

    void setPosition(Vector3 pos);

    void setRotation(Quaternion rot);

    void setScale(Vector3 scale);

    void onUpdate();

    bool onAction();

    void Appear(int trackIndex);

    void Disappear();
    //void Recover();

}
