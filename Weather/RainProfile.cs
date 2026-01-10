using UnityEngine;

[CreateAssetMenu(fileName = "New Rain Profile", menuName = "Ithaca/Rain Profile")]
public class RainProfile : ScriptableObject
{
    public ParticleSystem.MinMaxCurve emissionRateOverTime;
    public ParticleSystem.MinMaxCurve startSpeed;
    public float rainAngle;
}
