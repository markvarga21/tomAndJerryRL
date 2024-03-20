using System.Collections;
using UnityEngine;

public class HunterController : MonoBehaviour
{
    private float moveSpeed = 4f;
    private float delayTime = 0.5f;
    private bool canFollowPlayer = false;

    private Rigidbody rb;

    [SerializeField]
    private GameObject player;

    private Animator animator;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        ResetHunter();
    }

    public void FixedUpdate()
    {
        if (canFollowPlayer)
        {
            animator.SetBool("isMoving", true);
            Vector3 direction = (player.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void ResetHunter()
    {
        animator.SetBool("isMoving", false);
        canFollowPlayer = false;
        this.transform.localPosition = new Vector3(12.3f, 0f, -12.2f);
        this.transform.rotation = Quaternion.Euler(0, -45f, 0);
        StartCoroutine(StartFollowingPlayer());
    }

    private IEnumerator StartFollowingPlayer()
    {
        yield return new WaitForSeconds(delayTime);
        canFollowPlayer = true;
    }
}
