using UnityEngine;

public class CircleTransitionAnimator : MonoBehaviour
{

    private Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            anim.SetTrigger("Transition");
        }
    }
}
