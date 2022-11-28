using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ObserverOfSceneEndTrigger
{
    void OnNotify(int sceneIndex, bool sceneEnds);
}
