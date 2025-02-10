using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killscript : MonoBehaviour
{
    Animator ani;
    [SerializeField] float deathAnimationDuration = 2f; 

    void Start()
    {
        ani = GetComponent<Animator>();
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
    

    IEnumerator Die()
    {
        ani.SetBool("isDead", true);
        yield return new WaitForSeconds(deathAnimationDuration);
        Destroy(gameObject); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
