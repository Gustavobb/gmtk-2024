using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public enum ButtonType
    {
        Grower,
        Shrinker
    }

    [SerializeField] private ButtonType buttonType;
    [SerializeField] private ScalableObject scalable;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PowerUp")) return;
        Press(scalable);
    }

    private void Press(ScalableObject scalable)
    {
        switch (buttonType)
        {
            case ButtonType.Grower:
                scalable.ScaleUpQueue();
                break;
            case ButtonType.Shrinker:
                scalable.ScaleDownQueue();
                break;
        }
    }
}
