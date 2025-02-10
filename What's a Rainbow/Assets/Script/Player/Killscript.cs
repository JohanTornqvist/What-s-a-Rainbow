using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killscript : MonoBehaviour
{
    Animator ani;
    [SerializeField] float deathAnimationDuration = 2f;
    public Collider2D playerCollider;
    public Collider2D playerJumpBox;
    public GameObject player;
    public Rigidbody2D rb;
    private PlayerMovement playerMovement; // Reference to PlayerMovement script

    void Start()
    {
        ani = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        playerCollider = player.GetComponent<Collider2D>();
        playerMovement = player.GetComponent<PlayerMovement>(); // Get PlayerMovement script
    }

    public void StartDeathSequence()
    {
        StartCoroutine(Die());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("InstaDeath"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (collision.gameObject.CompareTag("Death"))
        {
            StartCoroutine(Die());
        }
    }

    public IEnumerator Die()
    {
        ani.SetBool("isDead", true);
        yield return new WaitForSeconds(0.2f); 
        playerCollider.enabled = false;
        playerJumpBox.enabled = false;
        rb.gravityScale = 0;
        if (playerMovement != null)
        {
            playerMovement.canMove = false;
            playerMovement.canJump = false;
        }
        yield return new WaitForSeconds(deathAnimationDuration - 0.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
