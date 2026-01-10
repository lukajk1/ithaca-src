using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Finds all MeshRenderers in the scene and replaces materials named "Lit" or "graybox"
/// with a specified replacement material.
/// </summary>
public class MaterialReplacer : MonoBehaviour
{
    [Tooltip("The material to replace 'Lit' or 'graybox' materials with.")]
    [SerializeField]
    private Material replacementMaterial;

    /// <summary>
    /// Searches the scene for MeshRenderers and updates specific materials.
    /// This method is intended to be run only in the Unity Editor.
    /// </summary>
    [Button]
    private void ReplaceMaterials()
    {
        // Ensure the replacement material is assigned
        if (replacementMaterial == null)
        {
            Debug.LogError("Replacement Material is not assigned.");
            return;
        }

        // Find all MeshRenderers in the scene using the modern API and specifying no sort order.
        MeshRenderer[] renderers = FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);
        int count = 0;

        foreach (MeshRenderer renderer in renderers)
        {
            // CRITICAL FIX: Use renderer.sharedMaterials to avoid creating material instances
            // that would cause leaks when running this function in the Editor.
            Material[] currentMaterials = renderer.sharedMaterials;
            bool materialsChanged = false;

            for (int i = 0; i < currentMaterials.Length; i++)
            {
                Material mat = currentMaterials[i];

                // Check if the material exists and if its name is an EXACT match for "Lit" or "graybox".
                if (mat != null)
                {
                    // Note: We use .name.Replace("(Instance)", "") to reliably check the asset name.
                    string matName = mat.name.Replace(" (Instance)", "");

                    if (matName == "Lit" || matName == "graybox")
                    {
                        // Replace the material in the local array copy
                        currentMaterials[i] = replacementMaterial;
                        materialsChanged = true;
                        count++;
                    }
                }
            }

            // Only assign the array back to the renderer if changes were made
            if (materialsChanged)
            {
                // CRITICAL FIX: Assign the modified array back to sharedMaterials
                renderer.sharedMaterials = currentMaterials;
            }
        }

        Debug.Log($"Material Replacement Complete: Replaced {count} materials across the scene.");
    }
}