using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ObserverOfSceneEndTrigger
{
    void OnNotify(int sceneIndex, bool sceneEnds);
}

public interface ObserverOfCameraMoveStarts
{
    void OnNotifyCameraMoveStarts();
}

public interface ObserverOfCameraMoveEnds
{
    void OnNotifyCameraMoveEnds();
}
