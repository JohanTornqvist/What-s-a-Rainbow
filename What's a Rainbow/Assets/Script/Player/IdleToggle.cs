using System.Collections;
using UnityEngine;

public class IdleToggle : MonoBehaviour
{
    private Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
        StartCoroutine(ToggleIdle2()); 
    }

    IEnumerator ToggleIdle2()
    {
        while (true) 
        {
            yield return new WaitForSeconds(11f); 
            ani.SetBool("isIdle2", true);
            Debug.Log("Why did the Chinese mountain bandit become a stand-up comedian?" +
                " Because he always \"robbed\" the audience of their silence!");
            yield return new WaitForSeconds(1f);
            ani.SetBool("isIdle2", false); 
        }
    }
}

