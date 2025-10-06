using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerFinal : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private Transform detectorSuelo;
    [SerializeField] private float radioDetector = 0.2f;
    [SerializeField] private LayerMask capaSuelo;

    private Vector2 ejeControl;
    private Rigidbody2D fisicas;
    private Animator animator;
    private SpriteRenderer sprite;
    private bool puedeMover = true;
    private bool enSuelo = false;

    [Header("Vida")]
    [SerializeField] private float vidaMax = 100f;
    private float vidaActual;
    [SerializeField] private Slider sliderVida;

    [Header("Ataque")]
    [SerializeField] private GameObject hitboxAtaque; // hitbox con collider trigger

    void Start()
    {
        animator = GetComponent<Animator>();
        fisicas = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        vidaActual = vidaMax;

        if (sliderVida != null)
        {
            sliderVida.maxValue = vidaMax;
            sliderVida.value = vidaActual;
        }

        if (hitboxAtaque != null)
            hitboxAtaque.SetActive(false);
    }

    // ----------- INPUT SYSTEM -----------
    public void OnMove(InputAction.CallbackContext context)
    {
        ejeControl = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!puedeMover) return;

        if (context.performed && enSuelo)
        {
            fisicas.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            enSuelo = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && puedeMover)
        {
            animator.SetTrigger("atacando");
        }
    }

    // ----------- UPDATE -----------
    void Update()
    {
        enSuelo = Physics2D.OverlapCircle(detectorSuelo.position, radioDetector, capaSuelo);
        animator.SetBool("enSuelo", enSuelo);

        if (!puedeMover) return;

        animator.SetBool("corriendo", ejeControl.x != 0);

        if (ejeControl.x < 0) sprite.flipX = true;
        else if (ejeControl.x > 0) sprite.flipX = false;
    }

    private void FixedUpdate()
    {
        if (!puedeMover) return;
        fisicas.linearVelocity = new Vector2(ejeControl.x * speed, fisicas.linearVelocity.y);
    }

    // ----------- VIDA -----------
    public void RecibirDanio(float cantidad)
    {
        if (!puedeMover) return;

        vidaActual -= cantidad;
        if (vidaActual < 0) vidaActual = 0;

        if (sliderVida != null)
            sliderVida.value = vidaActual;

        if (vidaActual <= 0)
            Morir();
    }

    private void Morir()
    {
        puedeMover = false;
        animator.SetTrigger("muerto");
        fisicas.linearVelocity = Vector2.zero;
        fisicas.bodyType = RigidbodyType2D.Static;
        Invoke("ReiniciarEscena", 1.5f);
    }

    private void ReiniciarEscena()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    // ----------- ATAQUE -----------
    public void ActivarHitbox()
    {
        if (hitboxAtaque != null)
            hitboxAtaque.SetActive(true);
    }

    public void DesactivarHitbox()
    {
        if (hitboxAtaque != null)
            hitboxAtaque.SetActive(false);
    }

    // ----------- DETECTAR GOLPES -----------
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            BossSimple boss = other.GetComponent<BossSimple>();
            if (boss != null)
            {
                boss.TomarDaño(20);
                Debug.Log("🗡️ Jugador golpeó al Boss!");
            }
        }
    }

    // ----------- GIZMOS -----------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (detectorSuelo != null)
            Gizmos.DrawWireSphere(detectorSuelo.position, radioDetector);
    }
}
