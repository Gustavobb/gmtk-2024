using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectUpdateManager : MonoBehaviour
{
    public delegate void UpdateDelegate();
    public static UpdateDelegate PerformUpdate;
    public static UpdateDelegate PerformFixedUpdate;

    private void Update()
    {
        if (PerformUpdate != null)
        {
            PerformUpdate();
        }
    }

    private void FixedUpdate()
    {
        if (PerformFixedUpdate != null)
        {
            PerformFixedUpdate();
        }
    }
}
