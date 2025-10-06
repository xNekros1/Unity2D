using UnityEngine;

public class PlataformMove : MonoBehaviour
{
    [SerializeField] private Transform[] puntos;
    private int puntoDeIndicio = 0;
    [SerializeField] private float speed = 2f;

    private void Update()
    {
        // Movimiento de la plataforma entre los puntos
        transform.position = Vector2.MoveTowards(transform.position, puntos[puntoDeIndicio].position, Time.deltaTime * speed);
        if (Vector2.Distance(puntos[puntoDeIndicio].transform.position, transform.position) < .1f)
        {
            puntoDeIndicio++;
            if (puntoDeIndicio >= puntos.Length) puntoDeIndicio = 0;
        }
    }

    // ---------------------- NUEVO: pegado del jugador ----------------------
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Cuando el jugador toca la plataforma, se hace hijo de la plataforma para moverse junto con ella
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Al salir de la plataforma, se quita la referencia al padre
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
