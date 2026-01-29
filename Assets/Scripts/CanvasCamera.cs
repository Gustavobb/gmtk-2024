using UnityEngine;

public class CanvasCamera : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Canvas>().planeDistance = 1f;
    }
}
