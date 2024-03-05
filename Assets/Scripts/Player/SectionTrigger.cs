using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    public GameObject roadSection;
    public SceneMove sceneMove; // Referencia al script SceneMove OJO

    private void Start()
    {
        // Obtener la referencia del script SceneMove
        sceneMove = FindObjectOfType<SceneMove>();
        if (sceneMove == null)
        {
            Debug.LogError("SceneMove script not found in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger"))
        {
            GameObject newRoadSection = Instantiate(roadSection, new Vector3(0, 0, 93f), Quaternion.identity);
            // Obtener la referencia del script SceneMove en la nueva instancia de la plataforma
            SceneMove platformMove = newRoadSection.GetComponent<SceneMove>();
            if (platformMove != null)
            {
                SceneMove.AddSceneMoveInstance(platformMove);
            }
        }
    }
}
