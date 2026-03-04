using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 8f;
    public Transform paddle;

    private Rigidbody2D rb;
    private bool launched = false;

    private Vector2 offset;

    
    private Collider2D deadZoneCol;
    private float deadZoneTopY;
    private bool deadzoneLock = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (paddle == null)
        {
            GameObject paddleObj = GameObject.Find("paddleRed_0");
            if (paddleObj != null) paddle = paddleObj.transform;
        }

        if (paddle != null)
            offset = (Vector2)transform.position - (Vector2)paddle.position;

        
        GameObject dz = GameObject.FindGameObjectWithTag("DeadZone");
        if (dz != null)
        {
            deadZoneCol = dz.GetComponent<Collider2D>();
            if (deadZoneCol != null)
                deadZoneTopY = deadZoneCol.bounds.max.y;
        }

        ResetBall();
    }

    void Update()
    {
        
        if (!launched && paddle != null)
        {
            rb.position = (Vector2)paddle.position + offset;

            if (Input.GetKeyDown(KeyCode.Space))
                Launch();
        }
    }

    void FixedUpdate()
    {
        
        if (launched && !deadzoneLock && deadZoneCol != null)
        {
            if (rb.position.y <= deadZoneTopY)
            {
                HandleFall();
            }
        }
    }

    void Launch()
    {
        launched = true;
        deadzoneLock = false;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = new Vector2(0.7f, 1f).normalized * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!launched) return;
        if (deadzoneLock) return;

        if (other.CompareTag("DeadZone"))
        {
            HandleFall();
        }
    }

    void HandleFall()
    {
        deadzoneLock = true;

        ResetBall();

        if (GameManager.Instance != null)
            GameManager.Instance.PerderVida();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Brick"))
        {
            Brick brick = collision.gameObject.GetComponent<Brick>();

            
            if (brick != null && brick.type == BrickType.Indestructible)
            {
                
                return;
            }

            
            if (brick != null && brick.type == BrickType.ExtraLife)
            {
                if (GameManager.Instance != null)
                    GameManager.Instance.GanharVida(1);
            }

            
            Destroy(collision.gameObject);

            if (GameManager.Instance != null)
                GameManager.Instance.BrickDestruido();
        }

        
        if (launched)
        {
            Vector2 v = rb.linearVelocity;
            if (Mathf.Abs(v.y) < 0.2f)
                v.y = Mathf.Sign(v.y == 0 ? 1f : v.y) * 0.2f;

            rb.linearVelocity = v.normalized * speed;
        }
    }

    void ResetBall()
    {
        launched = false;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        rb.bodyType = RigidbodyType2D.Kinematic;

        if (paddle != null)
            rb.position = (Vector2)paddle.position + offset;
    }
}