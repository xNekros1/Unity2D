using UnityEngine;
using UnityEngine.UI;

public class Colleccionable : MonoBehaviour
{
    [SerializeField]private int collectionLimit = 5;
    [SerializeField]private int collectionAmount = 0;
    [SerializeField] private Text textCollection;

    void Start()
    {
        ActualizarTexto();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Item")) return;

        if (collectionAmount >= collectionLimit)
        {
            Debug.Log("Límite alcanzado. No se puede recoger más items.");
            ActualizarTexto();
            return;
        }

        collectionAmount++;
        Destroy(collision.gameObject);
        Debug.Log("Item recolectado: " + collectionAmount);
        ActualizarTexto();
    }

    public void UsarItem()
    {
        if (collectionAmount > 0)
        {
            collectionAmount--;
            Debug.Log("Item usado. Ahora tienes: " + collectionAmount);
            ActualizarTexto();
        }
    }

    private void ActualizarTexto()
    {
        int faltan = collectionLimit - collectionAmount;
        textCollection.text = "ITEMS: " + collectionAmount + " | Faltan: " + faltan;
    }
}
