using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killscript : MonoBehaviour
{
    Animator ani;
    [SerializeField] float deathAnimationDuration = 2f;
    private Collider2D playerCollider;
    private Collider2D playerJumpBox;
    private GameObject player;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            ani = player.GetComponent<Animator>();
            playerCollider = player.GetComponent<Collider2D>();
            playerJumpBox = player.GetComponentInChildren<Collider2D>(); // Ensures jump box is found
            rb = player.GetComponent<Rigidbody2D>();
            playerMovement = player.GetComponent<PlayerMovement>();
        }
    }

    public void StartDeathSequence()
    {
        StartCoroutine(Die());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("InstaDeath"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (collision.gameObject.CompareTag("Death"))
        {
            StartCoroutine(Die());
            Debug.Log("Player has died.");
        }
    }

    private IEnumerator Die()
    {
        if (ani != null) ani.SetBool("isDead", true);
        yield return new WaitForSeconds(0.2f);

        if (playerCollider != null) playerCollider.enabled = false;
        if (playerJumpBox != null) playerJumpBox.enabled = false;

        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }

        if (playerMovement != null)
        {
            playerMovement.canMove = false;
            playerMovement.canJump = false;
        }

        yield return new WaitForSeconds(deathAnimationDuration - 0.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
