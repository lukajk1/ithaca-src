using StylizedWater3;
using UnityEngine;

public class BobberHitWaterTrigger : MonoBehaviour
{
    [SerializeField] AudioClip hitWaterSFX;
    [SerializeField] BobberManager bobber;
    [SerializeField] ParticleSystem splash;
    private bool contactingWater;
    private void OnTriggerEnter(Collider other)
    {
        // magic string.. fine but just have to remember to update if I ever change it
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            bobber.ClearPhysics();
            bobber.AlignToWater(true);

            if (!contactingWater)
            { 
                bobber.AddSagToLine();
                splash.transform.position = transform.position;
                splash.Play();
                FishingManager.i.Fish();
                SoundManager.Play(new SoundData(hitWaterSFX, SoundData.Type.SFX));

                contactingWater = true;
            }
        }
    }

    public void Reset()
    {
        contactingWater = false;
    }
}
