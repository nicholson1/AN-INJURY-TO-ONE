using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevelController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // used during development, reset this level when Z key is pressed down
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Reset This Level");
        }
    }

    public void ResetThisLevel()
    {
        Debug.Log("Reset This Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}
