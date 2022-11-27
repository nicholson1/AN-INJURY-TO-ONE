using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonController : MonoBehaviour
{
    // A static variable which holds a reference to the single created instance
    private static SingletonController instance = null;
    // For testing that we only call the constructor once
    private float randomNumber;

    // list of information that can be saved & loaded
    private Vector3 savedPlayerPosition;

    // A public static means of getting the reference to the single created instance, creating one if necessary
    public static SingletonController Instance
    {
        get
        {
            if (instance == null)
            {
                // Find singleton of this type in the scene
                var instance = GameObject.FindObjectOfType<SingletonController>();

                // If there is no singleton object in the scene, we have to add one
                if (instance == null)
                {
                    GameObject obj = new GameObject("Unity Singleton");
                    instance = obj.AddComponent<SingletonController>();

                    // Init the singleton
                    instance.FakeConstructor();

                    // The singleton object shouldn't be destroyed when we switch between scenes
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // Init the singleton
            instance.FakeConstructor();

            // The singleton object shouldn't be destroyed when we switch between scenes
            DontDestroyOnLoad(this.gameObject);
        }
        // Because we inherit from MonoBehaviour whem might have accidentally added several of them to the scene,
        // which will cause trouble, so we have to make sure we have just one!
        else
        {
            Destroy(gameObject);
        }
    }

    // Because this script inherits from MonoBehaviour, we cant use a constructor, so we have to invent our own
    private void FakeConstructor()
    {
        randomNumber = Random.Range(0f, 1f);

        // initiate saved information, all set to default value
        this.savedPlayerPosition = new Vector3(0, 0, 0);
    }

    // For testing
    public void TestSingleton()
    {
        Debug.Log($"Hello this is Singleton, my random number is: {randomNumber}");
    }

    public void UpdateSavedPlayerPosition(Vector3 newPlayerPosition)
    {
        this.savedPlayerPosition = newPlayerPosition;
        // this Debug.Log() ought to be put here, otherwise it will give the value before updating
        Debug.Log("saved player position: " + this.savedPlayerPosition.ToString());
    }

    public Vector3 ReturnSavedPlayerPosition()
    {
        Debug.Log("loaded player position: " + this.savedPlayerPosition.ToString());
        return this.savedPlayerPosition;
    }
}
