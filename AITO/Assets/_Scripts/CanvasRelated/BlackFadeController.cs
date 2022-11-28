using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackFadeController : MonoBehaviour
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

    public void PlayButton()
    {
        Debug.Log("Play Button Pressed");
        animator.SetTrigger("FadeOut");
    }

    public void ExitButton()
    {
        Debug.Log("Exit Button Pressed");
        Application.Quit();
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(1);
    }
}
