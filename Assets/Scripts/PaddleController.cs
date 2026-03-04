using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 10f;          
    public Collider2D movementArea;    

    private float minX;
    private float maxX;

    void Start()
    {
        if (movementArea != null)
        {
            
            Bounds b = movementArea.bounds;
            float halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
            minX = b.min.x + halfWidth;
            maxX = b.max.x - halfWidth;
        }
        else
        {
            
            minX = -3.5f;
            maxX =  3.5f;
        }
    }

    void Update()
    {
        
        float input = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(input, 0f, 0f) * speed * Time.deltaTime;
        transform.position += movement;

        
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}