using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Killscript : MonoBehaviour
{
    private Animator ani;
    private Collider2D playerCollider;
    private Collider2D playerJumpBox;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private DeathScreen deathScreen;

    public PlayerMovement playerMove;
    public PlayerDash playerDash;
    public PlayerInput playerInput;

    [SerializeField] private float deathAnimationDuration = 2f;
    [SerializeField] private bool showDeathScreen = true;
    public bool godMode = false;
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
            playerInput = player.GetComponent<PlayerInput>();
        }

        deathScreen = FindObjectOfType<DeathScreen>();
        if (deathScreen == null)
        {
            Debug.LogError("DeathScreen component not found in the scene.");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death") && godMode == false)
        {
            StartDeathSequence();
            Debug.Log("Player has died.");
        }

        if (collision.gameObject.CompareTag("EnemyDeath") && godMode == false)
        {
            StartDeathSequence();
            Debug.Log("Player has died.");
        }

        if (collision.gameObject.CompareTag("InstaDeath") && godMode == false)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void StartDeathSequence()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        ani.Play("DeathAnimation");
        ani.SetBool("isDead", true);
        if (playerMove != null) playerMove.enabled = false;
        if (playerDash != null) playerDash.enabled = false;
        if (playerInput != null) playerInput.enabled = false;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;  
        }
        if (playerCollider != null) playerCollider.enabled = false;
        if (playerJumpBox != null) playerJumpBox.enabled = false;
        yield return new WaitForSeconds(1.5f);
        if (showDeathScreen && deathScreen != null)
        {
            deathScreen.OpenDeathScreen();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void OnGodMode()
    {
        godMode = true;
    }

}
