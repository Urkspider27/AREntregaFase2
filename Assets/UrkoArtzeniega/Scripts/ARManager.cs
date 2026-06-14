using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Importante para no instanciar al tocar la UI

public class ARManager : MonoBehaviour
{
    [Header("AR Components")]
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    [Header("UI Elements")]
    public TextMeshProUGUI textNumPlanos;
    public Button btnBorrar;
    public TMP_Dropdown comboPrefabs;

    [Header("Prefabs a Instanciar")]
    public GameObject[] listaPrefabs; 

    private List<GameObject> objetosInstanciados = new List<GameObject>();

    void Start()
    {
        btnBorrar.onClick.AddListener(BorrarObjetos);
    }

    void Update()
    {
        textNumPlanos.text = "NumPlanos= " + planeManager.trackables.count;

        // Detección para PC
        if (Input.GetMouseButtonDown(0))
        {
            // Evitar instanciar si el usuario está tocando la UI
            if (EventSystem.current.IsPointerOverGameObject()) return;

            LanzarRayoAR(Input.mousePosition);
        }
        // Detección para Móvil
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Evitar instanciar si el usuario está tocando la UI
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

                LanzarRayoAR(touch.position);
            }
        }
    }

    void LanzarRayoAR(Vector2 posicionPantalla)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        // Lanzar raycast contra los planos detectados
        if (raycastManager.Raycast(posicionPantalla, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            InstanciarPrefabSeleccionado(hitPose.position, hitPose.rotation);
        }
    }

    void InstanciarPrefabSeleccionado(Vector3 posicion, Quaternion rotacion)
    {
        if (listaPrefabs.Length == 0) return;

        int indiceSeleccionado = comboPrefabs.value;
        GameObject prefabAInstanciar = listaPrefabs[indiceSeleccionado];

        // Instanciar y guardar la referencia en la lista
        GameObject nuevoObjeto = Instantiate(prefabAInstanciar, posicion, rotacion);
        objetosInstanciados.Add(nuevoObjeto);
    }

    public void BorrarObjetos()
    {
        foreach (GameObject obj in objetosInstanciados)
        {
            Destroy(obj);
        }
        objetosInstanciados.Clear();
    }
}