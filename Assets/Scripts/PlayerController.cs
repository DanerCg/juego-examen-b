using UnityEngine;
using UnityEngine.InputSystem;

/// Bart: izquierda = Flecha arriba, derecha = Espacio, salto = P
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpForce = 11f;

    [Header("Suelo")]
    public Transform groundCheck;          // objeto hijo en los pies de Bart
    public float groundRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("Sonidos (opcional)")]
    public AudioClip jumpSound;

    public bool CanControl { get; set; } = true;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    AudioSource audioSource;
    bool grounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null || !CanControl) return;

        float dir = 0f;
        if (kb.upArrowKey.isPressed) dir = -1f;   // izquierda
        if (kb.spaceKey.isPressed)   dir = 1f;    // derecha

        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        if (dir != 0f && sr != null) sr.flipX = dir < 0f;

        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        if (kb.pKey.wasPressedThisFrame && grounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            if (audioSource && jumpSound) audioSource.PlayOneShot(jumpSound);
        }

        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(dir));
            anim.SetBool("Grounded", grounded);
        }
    }

    // Golpe desde abajo contra un bloque
    void OnCollisionEnter2D(Collision2D col)
    {
        foreach (var contact in col.contacts)
        {
            // normal.y < 0 => el choque fue en la cabeza de Bart
            if (contact.normal.y < -0.5f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                var block = col.collider.GetComponent<BumpBlock>();
                if (block != null) block.Bump();
                if (anim != null) anim.SetTrigger("Hurt");
                break;
            }
        }
    }

    public void Die()
    {
        CanControl = false;
        rb.linearVelocity = Vector2.zero;
        if (anim != null) anim.SetTrigger("Die");
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
