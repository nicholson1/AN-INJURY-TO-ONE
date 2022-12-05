using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameManager : MonoBehaviour
{
    public static event Action<Vector3> MoveCamera;
    public bool LevelComplete = false;

    [SerializeField] private Transform[] CameraPositions;

    private int cameraPosIndex = 0;

    // JT: as a subject be observered
    //private List<ObserverOfCameraMoveStarts> _observerOfCameraMoveStarts = new List<ObserverOfCameraMoveStarts>();

    //public void AddObverserOfCameraMoveStarts(ObserverOfCameraMoveStarts observer)
    //{
    //    _observerOfCameraMoveStarts.Add(observer);
    //}

    //public void RemoveObverserOfCameraMoveStarts(ObserverOfCameraMoveStarts observer)
    //{
    //    _observerOfCameraMoveStarts.Remove(observer);
    //}

    //public void Notify()
    //{
    //    foreach (ObserverOfCameraMoveStarts observer in _observerOfCameraMoveStarts)
    //    {
    //        observer.OnNotifyCameraMoveStarts();
    //    }
    //}

    private void Start()
    {
        PuzzleTransitions.TransitionRight += TriggerTransition;

    }
    private void OnDestroy()
    {
        PuzzleTransitions.TransitionRight -= TriggerTransition;

    }

    private void TriggerTransition(bool isRight)
    {
        // JT: as a subject be observered
        //Notify();
        //Debug.Log("camera move starts");

        if (isRight)
        {
            Debug.Log("Go Right");
            cameraPosIndex += 1;
            MoveCamera(CameraPositions[cameraPosIndex].position);
        }
        else
        {
            Debug.Log("Go Left");

            cameraPosIndex -= 1;
            MoveCamera(CameraPositions[cameraPosIndex].position);
        }
    }
 

}
