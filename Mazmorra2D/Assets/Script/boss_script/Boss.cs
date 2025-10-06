using UnityEngine;
using UnityEngine.UI;

public class BossSimple : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform player;                 // Transform del jugador (tag "Player")
    [SerializeField] private Animator animator;
    [SerializeField] private Slider healthBar;                 // Slider para la barra de vida
    [SerializeField] private Vector3 healthBarOffset = new Vector3(0, 1.5f, 0);

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float chaseRange = 8f;           // Distancia máxima para seguir al jugador
    [SerializeField] private float attackRange = 2f;          // Distancia para atacar

    [Header("Vida")]
    [SerializeField] private float maxHealth = 500f;
    private float currentHealth;

    [Header("Ataque Melee")]
    [SerializeField] private Transform attackPoint;           // Empty frente al sprite
    [SerializeField] private float attackRadius = 1.5f;
    [SerializeField] private int damage = 20;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackCooldown = 2f;
    private float lastAttackTime = 0f;

    private bool mirandoDerecha = true;

    void Start()
    {
        // Buscar player automáticamente si no se asignó en inspector
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (animator == null)
            animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        if (player == null) return;

        float distancia = Vector2.Distance(transform.position, player.position);

        // Voltear sprite para mirar al jugador
        if ((player.position.x > transform.position.x && !mirandoDerecha) ||
            (player.position.x < transform.position.x && mirandoDerecha))
            Flip();

        // Solo seguir al jugador si está dentro del chaseRange y fuera del rango de ataque
        if (distancia <= chaseRange && distancia > attackRange)
        {
            animator.SetBool("corriendo", true);
            float dir = player.position.x - transform.position.x;
            transform.position += new Vector3(Mathf.Sign(dir) * moveSpeed * Time.deltaTime, 0f, 0f);
        }
        else
        {
            // Detenerse si no persigue
            animator.SetBool("corriendo", false);

            // Si está dentro del rango de ataque, intentar atacar
            if (distancia <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetTrigger("atacando");
                lastAttackTime = Time.time;
            }
        }

        // Actualizar la posición de la barra de vida
        UpdateHealthBarPosition();
    }

    // Voltear sprite horizontalmente
    private void Flip()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1f;
        transform.localScale = escala;
    }

    // Llamado desde Animation Event en el ataque
    public void RealizarAtaque()
    {
        if (attackPoint == null)
        {
            Debug.LogWarning("AttackPoint no asignado en BossSimple.");
            return;
        }

        // Detectar jugador dentro del radio del ataque
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);
        foreach (Collider2D p in hitPlayers)
        {
            PlayerFinal jugador = p.GetComponent<PlayerFinal>();
            if (jugador != null)
                jugador.RecibirDanio(damage);
        }
    }

    // Tomar daño
    public void TomarDaño(float daño)
    {
        currentHealth -= daño;
        if (currentHealth < 0) currentHealth = 0;
        UpdateHealthBar();

        if (currentHealth <= 0)
            Morir();
    }

    // Morir y destruir objeto después de animación
    private void Morir()
    {
        animator.SetTrigger("Muerto");
        Destroy(gameObject, 2f);
    }

    // Actualizar barra de vida (valor entre 0 y 1)
    private void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.value = currentHealth / maxHealth;
    }

    // Mover barra de vida sobre la cabeza del boss
    private void UpdateHealthBarPosition()
    {
        if (healthBar != null)
            healthBar.transform.position = transform.position + healthBarOffset;
    }

    // Gizmos para ver el radio de ataque en escena
    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}
