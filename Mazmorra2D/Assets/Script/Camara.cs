using UnityEngine;

public class Camara : MonoBehaviour
{
    [SerializeField] private Transform positionPlayer;
    private Vector3 posicionInicialCamara;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posicionInicialCamara = transform.position - positionPlayer.position;
        //player = GameObject.FindGameObjectWithTag("player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 nuevaPosicion = positionPlayer.position + posicionInicialCamara;
        transform.position = nuevaPosicion;
    }
}
