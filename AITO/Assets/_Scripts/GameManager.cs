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
        PuzzleTransitions.TransitionRight += TriggerTransition;

    }
    private void OnDestroy()
    {
        PuzzleTransitions.TransitionRight -= TriggerTransition;

    }

    private void TriggerTransition(bool isRight)
    {
        if (isRight)
        {
            cameraPosIndex += 1;
            MoveCamera(CameraPositions[cameraPosIndex].position);
        }
        else
        {
            cameraPosIndex -= 1;
            MoveCamera(CameraPositions[cameraPosIndex].position);
        }
        
    }
 

}
