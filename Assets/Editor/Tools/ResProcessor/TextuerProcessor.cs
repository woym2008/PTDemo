/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/29 14:27:43
 *	版 本：v 1.0
 *	描 述：贴图导入设置
* ========================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TextuerProcessor : AssetPostprocessor {

    public static bool isProcessTexture = true;

	// 图片导入时的设置
    void OnPreprocessTexture()
    {
        if(!isProcessTexture)
        {
            return;
        }

        TextureImporter importer = assetImporter as TextureImporter;
    }
}
