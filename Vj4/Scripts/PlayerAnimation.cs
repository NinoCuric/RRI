using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    public ParticleSystem dust;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float speed = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

        animator.SetFloat("Speed", speed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Jump");
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }

        if (speed > 0.1f)
        {
            if (!dust.isPlaying)
                dust.Play();
        }
        else
        {
            if (dust.isPlaying)
                dust.Stop();
        }
    }
}