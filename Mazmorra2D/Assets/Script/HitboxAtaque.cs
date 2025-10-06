using UnityEngine;

public class HitboxAtaque : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemigo"))
        {
            Destroy(other.gameObject); // ?? Destruye enemigo
        }
    }
}
