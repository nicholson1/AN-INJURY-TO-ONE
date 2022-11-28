using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneEndTriggerController : MonoBehaviour
{
    protected int sceneIndex;
    protected bool sceneEnds = false;

    private List<ObserverOfSceneEndTrigger> _observersOfSceneEndTrigger = new List<ObserverOfSceneEndTrigger>();

    public abstract void ReturnLog();
    public abstract int ReturnSceneIndex();

    public void AddObserverOfSceneEndTrigger(ObserverOfSceneEndTrigger observer)
    {
        _observersOfSceneEndTrigger.Add(observer);
    }

    public void RemoveObserverOfSceneEndTrigger(ObserverOfSceneEndTrigger observer)
    {
        _observersOfSceneEndTrigger.Remove(observer);
    }

    public void Notify(int sceneIndex, bool sceneEnds)
    {
        foreach (ObserverOfSceneEndTrigger observer in _observersOfSceneEndTrigger)
        {
            observer.OnNotify(sceneIndex, sceneEnds);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("This Scene Ends!");
        }
    }
}
