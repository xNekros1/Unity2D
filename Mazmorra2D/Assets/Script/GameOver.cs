using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;  // <-- cambié el nombre
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Perder()
    {
        animator.SetTrigger("gameOver");
        rb.bodyType = RigidbodyType2D.Static;
    }

    [SerializeField]
    private void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Espinas"))
        {
            Perder();
        }
    }
}