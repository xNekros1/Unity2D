using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Para reiniciar escena

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.value = currentHealth;
    }

    // Llamar a este método para recibir daño
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        healthBar.value = currentHealth;

        if (currentHealth == 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Aquí puedes poner animación de muerte si quieres
        Debug.Log("Has muerto");

        // Reinicia la escena después de 1 segundo
        Invoke("RestartGame", 1f);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
