using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChangeController : MonoBehaviour, ObserverOfSceneEndTrigger
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<Image>().enabled = true;
        //animator = GetComponent<Animator>();
        PrepareOnLoad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void PrepareOnLoad()
    {
        gameObject.GetComponent<Image>().enabled = true;
        this.animator = GetComponent<Animator>();

        foreach (SceneEndTriggerController controller in FindObjectsOfType<SceneEndTriggerController>())
        {
            controller.AddObserverOfSceneEndTrigger(this);
        }
    }

    public void OnNotify(int sceneIndex, bool sceneEnds)
    {
        if (sceneEnds)
        {
            this.animator.SetTrigger("FadeOut");
            OnFadeComplete(sceneIndex);
            Debug.Log("Scene " + sceneIndex + " Ends");
        }
    }

    public void OnFadeComplete()
    {
        // - 1: to the previous scene
        // + 1: to the next scene

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnFadeComplete(int sceneIndex)
    {
        // - 1: to the previous scene
        // + 1: to the next scene

        //SceneManager.LoadScene(sceneIndex - 1);
        SceneManager.LoadScene(sceneIndex + 1);
    }
}
