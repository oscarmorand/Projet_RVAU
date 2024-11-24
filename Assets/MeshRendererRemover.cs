using UnityEngine;

public class DisableMeshRenderersOnEditorOnlyLayer : MonoBehaviour
{
    void Start()
    {
        // Récupère tous les objets de la scène
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        // Parcourt tous les objets
        foreach (GameObject obj in allObjects)
        {
            // Vérifie si l'objet est sur la couche "EditorOnly"
            if (obj.layer == LayerMask.NameToLayer("EditorOnly"))
            {
                // Récupère le MeshRenderer de l'objet
                MeshRenderer renderer = obj.GetComponent<MeshRenderer>();

                // Désactive le MeshRenderer si l'objet en a un
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
            }
        }
    }
}
