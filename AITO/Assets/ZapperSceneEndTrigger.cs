using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapperSceneEndTrigger : SceneEndTriggerController
{
    public override void ReturnLog()
    {
        //throw new System.NotImplementedException();
        Debug.Log("Zapper Level End Observered");
    }

    public override int ReturnSceneIndex()
    {
        //throw new System.NotImplementedException();
        return this.sceneIndex;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.sceneIndex = 4;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.sceneEnds = true;
            Notify(this.sceneIndex, this.sceneEnds);
        }
    }
}
