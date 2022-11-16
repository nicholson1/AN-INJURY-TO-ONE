using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GameManager : MonoBehaviour
{
    public static event Action<Vector3> MoveCamera;
    public bool LevelComplete = false;

    [SerializeField] private Transform[] CameraPositions;

    private int cameraPosIndex =0;

    private void Start()
    {
        PuzzleTransitions.Transition += TriggerTransition;

    }
    private void OnDestroy()
    {
        PuzzleTransitions.Transition -= TriggerTransition;

    }

    private void TriggerTransition()
    {
        cameraPosIndex += 1;
        MoveCamera(CameraPositions[cameraPosIndex].position);
    }
 

}
