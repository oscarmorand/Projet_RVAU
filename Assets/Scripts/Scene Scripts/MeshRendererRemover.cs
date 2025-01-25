using UnityEngine;

public class MeshRendererRemover : MonoBehaviour
{
    void Start()
    {
        // R�cup�re tous les objets de la sc�ne
        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        // Parcourt tous les objets
        foreach (GameObject obj in allObjects)
        {
            // V�rifie si l'objet est sur la couche "EditorOnly"
            if (obj.layer == LayerMask.NameToLayer("EditorOnly"))
            {
                // R�cup�re le MeshRenderer de l'objet
                MeshRenderer renderer = obj.GetComponent<MeshRenderer>();

                // D�sactive le MeshRenderer si l'objet en a un
                if (renderer != null)
                {
                    renderer.enabled = false;
                }
            }
        }
    }
}
