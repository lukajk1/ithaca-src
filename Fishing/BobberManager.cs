using GogoGaga.OptimizedRopesAndCables;
using UnityEngine;

public class BobberManager : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private StylizedWater3.AlignToWater align;
    [SerializeField] private ParticleSystem ripple;
    [SerializeField] private Rope fishingLine;
    [SerializeField] private Transform player;
    [SerializeField] BobberHitWaterTrigger trigger;
    private Quaternion originalRot;

    private void Awake()
    {
        originalRot = rb.transform.rotation;
    }
    public void AlignToWater(bool value)
    {
        if (true)
        {
            rb.useGravity = false;

            Vector3 vec = rb.transform.position;
            vec = new Vector3(vec.x, ripple.transform.position.y, vec.z);

            //ripple.Stop(false);
            ripple.transform.position = vec;
            //ripple.Play();  
        }

        // Reset rotation
        rb.transform.rotation = Quaternion.identity;

        align.enabled = value;
    }
    public void MoveToPos(Vector3 pos)
    {
        rb.MovePosition(pos);
    }
    public void Cast(Vector3 forceDir, ForceMode forceMode)
    {
        AlignToWater(false);
        rb.useGravity = true;
        rb.AddForce(forceDir, forceMode);
    }
    public void ClearPhysics()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void AddSagToLine()
    {
        float dist = Vector3.Distance(player.position, rb.position);
        dist *= 1.06f;

        fishingLine.ropeLength = dist;
    }

    public void Reset()
    {
        trigger.Reset();
        fishingLine.ropeLength = 6f;
    }
}
