using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // create the instance at the beginning of the game
        SingletonController instance = SingletonController.Instance;
        //instance.TestSingleton();

        // for proving all the instances are the same one (random number is the same)
        //SingletonController instance1 = SingletonController.Instance;
        //instance1.TestSingleton();
        //SingletonController instance2 = SingletonController.Instance;
        //instance2.TestSingleton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
