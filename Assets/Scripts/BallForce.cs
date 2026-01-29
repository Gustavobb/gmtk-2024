using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PushOnScalableTouch2D : MonoBehaviour
{
    [SerializeField] private float pushForce = 1f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if ((!collision.gameObject.CompareTag("Scalable"))||(collision.gameObject.GetComponent<ScalableObject>().isScaling==false)||((collision.gameObject.GetComponent<ScalableObject>().isBouncer)==false))
        if (collision.gameObject.CompareTag("PowerUpBouncer"))
        {
            // reflect the ball
            rb.velocity = 1.5f * (collision.contacts[0].normal + rb.velocity).normalized;
            return;
        }
        
        ScalableObject scalable = collision.gameObject.GetComponent<ScalableObject>();
        if (!scalable) return;

        if (scalable && !scalable.isScaling)
        {
            return;
        }
        
        Vector2 direction = (Vector2)(transform.position - collision.transform.position);
        direction.Normalize();
        rb.AddForce(direction * pushForce, ForceMode2D.Impulse);
    }
}
