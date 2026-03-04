using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 10f;          // velocidade do paddle
    public Collider2D movementArea;    // arraste aqui o PlayerArea (BoxCollider2D)

    private float minX;
    private float maxX;

    void Start()
    {
        if (movementArea != null)
        {
            // pega os limites da área do jogador
            Bounds b = movementArea.bounds;
            float halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
            minX = b.min.x + halfWidth;
            maxX = b.max.x - halfWidth;
        }
        else
        {
            // caso não arraste o movementArea, usa um limite fixo
            minX = -3.5f;
            maxX =  3.5f;
        }
    }

    void Update()
    {
        // entrada A/D ou setas
        float input = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(input, 0f, 0f) * speed * Time.deltaTime;
        transform.position += movement;

        // limita dentro da área
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}