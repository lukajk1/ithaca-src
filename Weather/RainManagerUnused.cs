using UnityEngine;

// This ensures the component can only be added to GameObjects 
// that already have a ParticleSystem.
[RequireComponent(typeof(ParticleSystem))]
public class RainManagerUnused : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private Quaternion _initialLocalRotation;

    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();

        // Store the initial rotation of the particle system before applying the profile
        _initialLocalRotation = transform.localRotation;
    }

    /// <summary>
    /// Applies the settings from a given RainProfile to the Particle System.
    /// This method can be called publicly at runtime to swap profiles.
    /// </summary>
    /// <param name="profileToApply">The RainProfile containing the new settings.</param>
    public void ApplyProfile(RainProfile profileToApply)
    {
        if (profileToApply == null)
        {
            Debug.LogError("The RainProfile passed to ApplyProfile is null on " + gameObject.name, this);
            return;
        }

        // --- 1. Apply Emission Rate over Time ---
        var emission = _particleSystem.emission;
        emission.rateOverTime = profileToApply.emissionRateOverTime;

        // --- 2. Apply Start Speed ---
        var main = _particleSystem.main;
        main.startSpeed = profileToApply.startSpeed;

        // --- 3. Apply Rain Angle (Relative X-Axis Rotation) ---

        // Start with the initial rotation's Y and Z components (to maintain facing/roll)
        Vector3 finalEuler = _initialLocalRotation.eulerAngles;

        // Set the X component to the profile's rain angle. 
        // Unity's rotation is applied as (X, Y, Z), so changing the X angle 
        // controls the slope of the rain relative to the ground.
        finalEuler.x = profileToApply.rainAngle;

        // Apply the new rotation
        transform.localRotation = Quaternion.Euler(finalEuler);
    }
}
