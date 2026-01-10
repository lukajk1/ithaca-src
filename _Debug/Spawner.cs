using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Assign the GameObject/Prefab you want to spawn in the Inspector
    public GameObject objectToInstantiate;

    void Update()
    {
        // Check if the 'Y' key is pressed down
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        if (objectToInstantiate == null)
        {
            Debug.LogError("objectToInstantiate is not assigned in the Inspector.");
            return;
        }

        // Pull the current world position and rotation of this Spawner object
        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = transform.rotation;

        // Instantiate the object at the pulled world position and rotation, with no parent
        Instantiate(objectToInstantiate, spawnPosition, spawnRotation, null);
    }
}
