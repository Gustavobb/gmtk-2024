using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    private int shaderPropertyID = Shader.PropertyToID("_Fade");

    public void AnimateFade(float targetFloat)
    {
        StartCoroutine(Fade(targetFloat));
    }

    private IEnumerator Fade(float targetFloat)
    {
        float currentFloat = outlineMaterial.GetFloat(shaderPropertyID);
        float elapsedTime = 0;
        float duration = .5f;

        while (elapsedTime < duration)
        {
            outlineMaterial.SetFloat(shaderPropertyID, Mathf.Lerp(currentFloat, targetFloat, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        outlineMaterial.SetFloat(shaderPropertyID, targetFloat);
    }
}
