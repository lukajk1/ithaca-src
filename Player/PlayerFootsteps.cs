using UnityEngine;
using UnityEngine.VFX;

public class PlayerFootsteps : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GroundedHitbox groundedHitbox;
    [SerializeField] private PlayerLookAndMove lookAndMove;

    [Header("sfx")]
    [SerializeField] private AudioClip footstepDefault;
    [SerializeField] private AudioClip footstepMetal;
    [SerializeField] private AudioClip footstepWater;


    [SerializeField] float stepDistance = 220f; // units in between footsteps
    float distanceMoved;
    bool isMoving;

    Vector3 lastFootstepPos;

    private void OnEnable()
    {
        PlayerLookAndMove.OnLandingFromJump += OnLandingFromJump;
    }

    private void OnDisable()
    {
        PlayerLookAndMove.OnLandingFromJump -= OnLandingFromJump;
    }

    private void Start()
    {
        lastFootstepPos = transform.position;
    }
    private void FixedUpdate()
    {
        isMoving = rb.linearVelocity.sqrMagnitude > 0.05f;

        if(groundedHitbox.IsGrounded() && groundedHitbox.materialType == ObjectMaterial.Type.Water)
        {
            lookAndMove.ResetToJumpSavePos();
        } 

        if (!groundedHitbox.IsGrounded())
        {
            lastFootstepPos = transform.position;
            distanceMoved = 0;
        }

        if (groundedHitbox.IsGrounded() && isMoving)
        {
            distanceMoved += Vector3.Distance(transform.position, lastFootstepPos);
            if (distanceMoved >= stepDistance)
            {
                SoundManager.Play(new SoundData(GetClip(), SoundData.Type.SFX));

                lastFootstepPos = transform.position;
                distanceMoved = 0;
            }
        }


    }

    private void OnLandingFromJump()
    {
        SoundManager.Play(new SoundData(GetClip(), SoundData.Type.SFX));
    }

    private AudioClip GetClip()
    {
        switch (groundedHitbox.materialType)
        {
            case ObjectMaterial.Type.Concrete:
                return footstepDefault;
            case ObjectMaterial.Type.Metal:
                return footstepMetal;
            case ObjectMaterial.Type.Water:
                return footstepWater;
            case ObjectMaterial.Type.Default:
                return footstepDefault;
            default:
                return footstepDefault;
        }
    }
}
