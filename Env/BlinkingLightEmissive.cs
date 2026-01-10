using System.Collections;
using UnityEngine;

public class BlinkingLightEmissive : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    [SerializeField] private float blinkDuration = 0.3f;

    [SerializeField] private float randDelayMin = 1f;
    [SerializeField] private float randDelayMax = 5f;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        StartCoroutine(BlinkCR());
    }

    IEnumerator BlinkCR()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(randDelayMin, randDelayMax));

            int numOfBlinks = Random.Range(1, 3);
            for (int i = 0; i < numOfBlinks; i++)
            {
                meshRenderer.enabled = false;
                yield return new WaitForSeconds(blinkDuration / 2f);

                meshRenderer.enabled = true;
                yield return new WaitForSeconds(blinkDuration / 2f);
            }

        }
    }

}