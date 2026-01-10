using NaughtyAttributes;
using UnityEngine;

public class ShapeGen : MonoBehaviour
{
    [SerializeField] private GameObject side;

    [Button]
    private void GenerateShape()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
#if UNITY_EDITOR
        DestroyImmediate(transform.GetChild(i).gameObject);
#else
        Destroy(transform.GetChild(i).gameObject);
#endif
        }

        int sides = Random.Range(3, 7);

        float rotAngle = 360f / (float)sides;
        for (int i = 0; i < sides; i++)
        {
            GameObject sideNew = Instantiate(side, transform);
            sideNew.transform.rotation = Quaternion.Euler(0, 0, rotAngle * (float)i);
        }
    }
}
