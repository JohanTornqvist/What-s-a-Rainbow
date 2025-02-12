using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killscript : MonoBehaviour
{
    private Animator ani;
    [SerializeField] private float deathAnimationDuration = 2f;
    private Collider2D playerCollider;
    private Collider2D playerJumpBox;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private DeathScreen deathScreen;

    [SerializeField] private bool showDeathScreen = true;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            ani = player.GetComponent<Animator>();
            playerCollider = player.GetComponent<Collider2D>();
            playerJumpBox = player.GetComponentInChildren<Collider2D>();
            rb = player.GetComponent<Rigidbody2D>();
            playerMovement = player.GetComponent<PlayerMovement>();
        }

        deathScreen = FindObjectOfType<DeathScreen>();
        if (deathScreen == null)
        {
            Debug.LogError("DeathScreen component not found in the scene.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("InstaDeath"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (collision.gameObject.CompareTag("Death"))
        {
            StartCoroutine(Die());
            Debug.Log("Player has died.");
        }
    }

    public void StartDeathSequence()
    {
        StartCoroutine(Die());
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

        
        if (showDeathScreen && deathScreen != null)
        {
            deathScreen.OpenDeathScreen();  
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  
        }
    }
}
