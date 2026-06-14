using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenu : MonoBehaviour
{
    private bool dentroRojo = false;
    private bool dentroAzul = false;

    void OnTriggerEnter(Collider other)
    {
        // Detectar si la cámara choca con el muro rojo o azul
        if (other.gameObject.name == "MuroRojo" || other.gameObject.name == "Escena1_Transportar")
        {
            dentroRojo = true;
        }
        else if (other.gameObject.name == "MuroAzul" || other.gameObject.name == "Escena2_Transportar")
        {
            dentroAzul = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Detectar si la cámara se aleja de los muros
        if (other.gameObject.name == "MuroRojo" || other.gameObject.name == "Escena1_Transportar")
        {
            dentroRojo = false;
        }
        else if (other.gameObject.name == "MuroAzul" || other.gameObject.name == "Escena2_Transportar")
        {
            dentroAzul = false;
        }
    }

    void OnGUI()
    {
        // Botón flotante nativo que no se congela por culpa del simulador de Unity 6
        if (dentroRojo)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 120, Screen.height / 2 - 25, 240, 50), "Ir a Escena Planos"))
            {
                ApagarARyViajar("Escena1_Planos");
            }
        }
        else if (dentroAzul)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 120, Screen.height / 2 - 25, 240, 50), "Ir a Escena Imagenes"))
            {
                ApagarARyViajar("Escena2_Imagenes");
            }
        }
    }

    void ApagarARyViajar(string destino)
    {
        // Desactivamos el motor de AR de esta escena para que no deje basura flotando en el cambio
        GameObject arSession = GameObject.Find("AR Session");
        if (arSession != null) arSession.SetActive(false);

        GameObject xrOrigin = GameObject.Find("XR Origin");
        if (xrOrigin != null) xrOrigin.SetActive(false);

        SceneManager.LoadScene(destino);
    }
}