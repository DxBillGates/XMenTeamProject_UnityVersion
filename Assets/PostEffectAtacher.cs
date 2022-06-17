using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffectAtacher : MonoBehaviour
{
    public Material highLumiMat;
    public Material blurMat;
    public Material compoMat;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        RenderTexture highLumiTex =RenderTexture.GetTemporary(source.width, source.width, 0, source.format);
        RenderTexture blurTex =RenderTexture.GetTemporary(source.width, source.width, 0, source.format);
        //縮小バッファ  
        RenderTexture buffer=RenderTexture.GetTemporary(blurTex.width/2,blurTex.height/2,0,source.format);
        

        Graphics.Blit(source, highLumiTex, highLumiMat);
        Graphics.Blit(highLumiTex, blurTex, blurMat);
        compoMat.SetTexture("_HighLumi", blurTex);
        Graphics.Blit(source, destination, compoMat);

        RenderTexture.ReleaseTemporary(blurTex);
        RenderTexture.ReleaseTemporary(highLumiTex);
    }
}
