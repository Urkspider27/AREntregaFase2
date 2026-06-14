using UnityEngine;
using UnityEngine.SceneManagement;

public class Navegacion : MonoBehaviour
{
    public void CambiarEscena(string nombreEscena)
    {
        // Apagamos los sistemas AR locales antes de volver al menú inicial
        GameObject arSession = GameObject.Find("AR Session");
        if (arSession != null) arSession.SetActive(false);

        GameObject xrOrigin = GameObject.Find("XR Origin");
        if (xrOrigin != null) xrOrigin.SetActive(false);

        SceneManager.LoadScene(nombreEscena);
    }
}