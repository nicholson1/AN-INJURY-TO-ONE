using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonController : MonoBehaviour
{
    public GameObject fadeOutImage;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = fadeOutImage.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButton()
    {
        Debug.Log("Play Button Pressed");
        animator.SetTrigger("FadeOut");
        SceneManager.LoadScene(1);
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
