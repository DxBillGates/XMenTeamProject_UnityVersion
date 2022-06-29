using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffectAtacher : MonoBehaviour
{
    bool isBloom { get; set; }
    public Material highLumiMat;
    public Material blurMat;
    public Material compoMat;
    private void Start()
    {
        isBloom = false;
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (isBloom)
        {
            RenderTexture highLumiTex = RenderTexture.GetTemporary(source.width, source.width, 0, source.format);
            RenderTexture blurTex = RenderTexture.GetTemporary(source.width, source.width, 0, source.format);
            //縮小バッファ  
            RenderTexture buffer1 = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, source.format);
            RenderTexture buffer2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, source.format);
            RenderTexture buffer3 = RenderTexture.GetTemporary(source.width / 8, source.height / 8, 0, source.format);


            Graphics.Blit(source, highLumiTex, highLumiMat);
            Graphics.Blit(highLumiTex, buffer1);
            Graphics.Blit(buffer1, buffer2, blurMat);
            Graphics.Blit(buffer2, buffer3, blurMat);
            Graphics.Blit(buffer3, blurTex);
            Graphics.Blit(blurTex, buffer3);
            Graphics.Blit(buffer3, buffer2);
            Graphics.Blit(buffer2, buffer1);

            compoMat.SetTexture("_HighLumi", buffer1);
            Graphics.Blit(source, destination, compoMat);

            RenderTexture.ReleaseTemporary(buffer1);
            RenderTexture.ReleaseTemporary(buffer2);
            RenderTexture.ReleaseTemporary(buffer3);
            RenderTexture.ReleaseTemporary(blurTex);
            RenderTexture.ReleaseTemporary(highLumiTex);
        }
        else
        {
            Graphics.Blit(source, destination);

        }
    }
}
