using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class Util
{
    public static void WaitToPerform(float seconds, System.Action action)
    {
        Player.Instance.StartCoroutine(WaitToPerformCoroutine(seconds, action));
    }
    
    public static void WaitForFramesToPerform(int frames, System.Action action)
    {
        Player.Instance.StartCoroutine(WaitForFramesCoroutine(frames, action));
    }
    
    private static IEnumerator WaitToPerformCoroutine(float seconds, System.Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }
    
    public static IEnumerator WaitForFramesCoroutine(int frames, System.Action action)
    {
        yield return new WaitForEndOfFrameUnit();

        for (int i = 0; i < frames; i++)
        {
            yield return new WaitForNextFrameUnit();
        }

        action?.Invoke();
    }
    
    public static IEnumerator AnimateShaderFloatProperty(List<Material> materials, int propertyID, float to, float duration, Action callback)
    {
        float from = materials[0].GetFloat(propertyID);
        yield return Player.Instance.StartCoroutine(AnimateFloat((float val) =>
        {
            foreach (var material in materials)
            {
                material.SetFloat(propertyID, val);
            }

            return 0;
        }, from, to, duration, callback));
        
        callback?.Invoke();
    }

    public static IEnumerator AnimateFloat(Func<float, float> func, float val, float finalVal, float fadeDuration, Action o)
    {
        float elapsed = 0f;
        float initialVal = val;
        while (elapsed < fadeDuration)
        {
            float t = elapsed / fadeDuration;
            t = Easing.EaseOutCircular(t);
            float value = Mathf.Lerp(initialVal, finalVal, t);
            elapsed += Time.deltaTime;
            
            func(value);
            yield return null;
        }

        func(finalVal);
        o?.Invoke();
    }
}
