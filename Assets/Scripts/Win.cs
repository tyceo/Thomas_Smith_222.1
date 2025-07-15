using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Win : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DelayedAction());
    }

    private IEnumerator DelayedAction()
    {
        yield return new WaitForSeconds(3f); // Wait for 3 seconds
        Debug.Log("This runs after 3 seconds!");
        SceneManager.LoadScene("Game");
    }

    


}
