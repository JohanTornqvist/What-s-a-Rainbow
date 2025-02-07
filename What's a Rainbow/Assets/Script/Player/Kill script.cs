using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killscript : MonoBehaviour
{
    Animator ani;
    [SerializeField] float deathAnimationDuration = 2.75f; 

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    public void StartDeathSequence()
    {
        StartCoroutine(Die()); 
    }

    IEnumerator Die()
    {
        ani.SetBool("isDead", true); 
        yield return new WaitForSeconds(deathAnimationDuration); 
        Destroy(gameObject); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}
