using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad;
    [SerializeField] private float fuerzaSalto;
    private Vector2 ejeControl;
    private Rigidbody2D fisicas;
    private Animator animator;
    private SpriteRenderer sprite;

    [Header("Vida")]
    [SerializeField] private float vidaMax = 100;
    private float vidaActual;
    [SerializeField] private Slider sliderVida; // Arrastrar el Slider del Canvas aquí

    [Header("Ataque")]
    [SerializeField] private GameObject hitboxAtaque; // hitbox hijo del Player

    void Start()
    {
        fisicas = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        vidaActual = vidaMax;
        if (sliderVida != null)
        {
            sliderVida.maxValue = vidaMax;
            sliderVida.value = vidaActual;
        }

        // desactivamos el hitbox al iniciar
        if (hitboxAtaque != null)
            hitboxAtaque.SetActive(false);
    }

    // ------------------ INPUT ------------------
    public void OnMove(InputAction.CallbackContext context)
    {
        ejeControl = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && animator.GetBool("enSuelo"))
        {
            fisicas.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetTrigger("atacando");
        }
    }

    // ------------------ UPDATE ------------------
    void Update()
    {
        Vector2 movimiento = new Vector2(ejeControl.x, 0);
        transform.Translate(movimiento * velocidad * Time.deltaTime);

        animator.SetBool("corriendo", ejeControl.x != 0);

        if (ejeControl.x < 0) sprite.flipX = true;
        else if (ejeControl.x > 0) sprite.flipX = false;
    }

    // ------------------ COLISIONES ------------------
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            animator.SetBool("enSuelo", true);
        }

        if (collision.gameObject.CompareTag("Enemigo"))
        {
            RecibirDanio(10); // daño por enemigos
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            animator.SetBool("enSuelo", false);
        }
    }

    // ------------------ VIDA ------------------
    public void RecibirDanio(float cantidad)
    {
        vidaActual -= cantidad;
        if (vidaActual < 0) vidaActual = 0;

        if (sliderVida != null)
        {
            sliderVida.value = vidaActual;
        }

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        animator.SetTrigger("muerto");
        fisicas.bodyType = RigidbodyType2D.Static;
        Invoke("ReiniciarEscena", 1.5f);
    }

    private void ReiniciarEscena()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    // ------------------ ATAQUE ------------------
    // llamados desde eventos de la animación de ataque
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
}
