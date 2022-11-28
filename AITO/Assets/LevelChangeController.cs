using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangeController : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnFadeComplete()
    {
        // for playtest, will return to menu scene
        // need to replaced to "+ 1" after having following scenes
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
