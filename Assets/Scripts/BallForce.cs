using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PushOnScalableTouch2D : MonoBehaviour
{
    [SerializeField] private float pushForce = 1f;

    private Rigidbody2D rb;
    private Vector2 previousVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        previousVelocity = rb.linearVelocity;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if ((!collision.gameObject.CompareTag("Scalable"))||(collision.gameObject.GetComponent<ScalableObject>().isScaling==false)||((collision.gameObject.GetComponent<ScalableObject>().isBouncer)==false))
        if (collision.gameObject.CompareTag("PowerUpBouncer"))
        {
            print(previousVelocity);
            // reflect the ball
            rb.linearVelocity = pushForce * Vector2.Reflect(previousVelocity.normalized, collision.collider.transform.up);
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
