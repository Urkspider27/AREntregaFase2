using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARManager : MonoBehaviour
{
    [Header("AR Components")]
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    [Header("UI Elements")]
    public TMPro.TextMeshProUGUI textNumPlanos;
    public Button btnBorrar;
    public TMPro.TMP_Dropdown comboPrefabs;

    [Header("Prefabs a Instanciar")]
    public List<GameObject> listaPrefabs;

    void Start()
    {
        // LIMPIEZA DRÁSTICA: Destruimos cualquier cámara fantasma del menú anterior
        GameObject[] camarasEnEscena = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (GameObject cam in camarasEnEscena)
        {
            if (cam.gameObject != Camera.main.gameObject && cam.transform.root.name.Contains("DontDestroyOnLoad"))
            {
                Destroy(cam.gameObject);
            }
        }

        // Forzar inicialización de interfaz limpia
        if (textNumPlanos != null)
        {
            textNumPlanos.text = "NumPlanos= 0";
        }
    }

    void Update()
    {
        // Actualizar el contador de planos detectados en tiempo real
        if (textNumPlanos != null && planeManager != null)
        {
            textNumPlanos.text = "NumPlanos= " + planeManager.trackables.count;
        }

        // Detección para PC (Clic del ratón)
        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
            LanzarRayoAR(Input.mousePosition);
        }
        // Detección para Móvil (Toque táctil para el APK definitivo)
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;
                LanzarRayoAR(touch.position);
            }
        }
    }

    void LanzarRayoAR(Vector2 posicionPantalla)
    {
        if (raycastManager == null) return;

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(posicionPantalla, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            InstanciarPrefabSeleccionado(hitPose.position, hitPose.rotation);
        }
    }

    void InstanciarPrefabSeleccionado(Vector3 posicion, Quaternion rotacion)
    {
        if (comboPrefabs == null || listaPrefabs == null) return;

        // Leer la posición del desplegable (0 = Rojo, 1 = Azul, 2 = Verde)
        int indiceSeleccionado = comboPrefabs.value;

        if (indiceSeleccionado >= 0 && indiceSeleccionado < listaPrefabs.Count)
        {
            GameObject prefabAEscoger = listaPrefabs[indiceSeleccionado];
            if (prefabAEscoger != null)
            {
                Instantiate(prefabAEscoger, posicion, rotacion);
            }
        }
    }
}