using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureManager : MonoBehaviour
{
    public RenderTexture renderTexture;

    private void Update()
    {
        if (Screen.width != renderTexture.width || Screen.height != renderTexture.height)
        {
            renderTexture.Release();
            renderTexture.width = Screen.width;
            renderTexture.height = Screen.height;
            renderTexture.Create();
        }
    }
}
