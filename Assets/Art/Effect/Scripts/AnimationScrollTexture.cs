using UnityEngine;
using System.Collections;

public class AnimationScrollTexture : MonoBehaviour {


    private Material mRenderMat = null;

    public float SpeedX = 0.25f;
    public float SpeedY = 0.25f;

    float offsetX = 0.0f;
    float offsetY = 0.0f;


    void Awake()
    {
        mRenderMat = GetComponent<Renderer>().material;
    }

    Vector2 offset = new Vector2();
    void Update()
    {
        offsetX += Time.deltaTime * SpeedX;
        offsetY += Time.deltaTime * SpeedY;

        offsetX = Mathf.Repeat(offsetX,1.0f);
        offsetY = Mathf.Repeat(offsetY,1.0f);

        //GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offsetX, offsetY);
        //gc优化 by yuanquanwei
        offset.Set(offsetX, offsetY);
        mRenderMat.mainTextureOffset = offset;
    }

}
