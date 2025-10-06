using UnityEngine;
using UnityEngine.UI;

public class DestruirEnemigos : MonoBehaviour
{
    private int eliminados = 0;
    [SerializeField] private Text textEliminados;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colision detectada con: " + collision.name); // para depurar
        if (collision.CompareTag("Destroy"))
        {
            eliminados++;
            if (textEliminados != null)
                textEliminados.text = "Destoy: " + eliminados;

            Destroy(collision.gameObject);
            Debug.Log("Objeto destruido");
        }
    }
}
