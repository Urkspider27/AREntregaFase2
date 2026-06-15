using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenu : MonoBehaviour
{
    private bool dentroRojo = false;
    private bool dentroAzul = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "MuroRojo") dentroRojo = true;
        else if (other.gameObject.name == "MuroAzul") dentroAzul = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "MuroRojo") dentroRojo = false;
        else if (other.gameObject.name == "MuroAzul") dentroAzul = false;
    }

    void OnGUI()
    {
        if (dentroRojo)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 120, Screen.height / 2 - 25, 240, 50), "Ir a Escena Planos"))
            {
                SceneManager.LoadScene("Escena1_Planos");
            }
        }
        else if (dentroAzul)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 120, Screen.height / 2 - 25, 240, 50), "Ir a Escena Imagenes"))
            {
                SceneManager.LoadScene("Escena2_Imagenes");
            }
        }
    }
}