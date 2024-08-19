using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    private const string CONNECTOR_TAG = "Connector";
    private const string SCALABLE_TAG = "Scalable";
    public enum ConnectorType
    {
        Grower,
        Shrinker
    }

    [SerializeField] private ConnectorType connectorType;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(CONNECTOR_TAG) || collision.gameObject.CompareTag(SCALABLE_TAG))
        {
            ScalableObject scalable = collision.gameObject.GetComponent<ScalableObject>();
            if (scalable.isScaling) return;

            switch (connectorType)
            {
                case ConnectorType.Grower:
                    scalable.ScaleUpQueue();
                    break;
                case ConnectorType.Shrinker:
                    scalable.ScaleDownQueue();
                    break;
            }
        }
    }
}
