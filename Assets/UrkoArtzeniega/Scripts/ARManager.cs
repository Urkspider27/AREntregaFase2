using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARManager : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    public TMPro.TextMeshProUGUI textNumPlanos;
    public Button btnBorrar;
    public TMPro.TMP_Dropdown comboPrefabs;

    public List<GameObject> listaPrefabs;

    // Lista para trackear lo instanciado
    private List<GameObject> cubosCreados = new List<GameObject>();

    void Start()
    {
        if (textNumPlanos != null) textNumPlanos.text = "NumPlanos= 0";

        // Asigna la funcion de borrar al boton
        if (btnBorrar != null)
        {
            btnBorrar.onClick.RemoveAllListeners();
            btnBorrar.onClick.AddListener(BorrarTodosLosCubos);
        }
    }

    void Update()
    {
        if (textNumPlanos != null && planeManager != null)
        {
            textNumPlanos.text = "NumPlanos= " + planeManager.trackables.count;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
            LanzarRayoAR(Input.mousePosition);
        }
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
            InstanciarPrefabSeleccionado(hits[0].pose.position, hits[0].pose.rotation);
        }
    }

    void InstanciarPrefabSeleccionado(Vector3 posicion, Quaternion rotacion)
    {
        if (comboPrefabs == null || listaPrefabs == null) return;

        int indice = comboPrefabs.value;
        if (indice >= 0 && indice < listaPrefabs.Count)
        {
            if (listaPrefabs[indice] != null)
            {
                // Guardamos el cubo en la lista al crearlo
                GameObject nuevoCubo = Instantiate(listaPrefabs[indice], posicion, rotacion);
                cubosCreados.Add(nuevoCubo);
            }
        }
    }

    // Funcion que vacia la escena
    public void BorrarTodosLosCubos()
    {
        foreach (GameObject cubo in cubosCreados)
        {
            if (cubo != null) Destroy(cubo);
        }
        cubosCreados.Clear();
    }
}