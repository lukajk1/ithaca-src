using UnityEngine;

public class ObjectMaterial : MonoBehaviour
{
    public enum Type
    {
        Concrete = 5, 
        Metal = 10,
        Wood = 15,


        Water = 990,
        Default = 999,
    }

    public Type type = Type.Default;
}
