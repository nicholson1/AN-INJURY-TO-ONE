using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Vector3 targetPos;
    private bool isMoving = false;
    [SerializeField] private float MoveSpeed;

    // JT: as a subject be observered
    private List<ObserverOfCameraMoveEnds> _observerOfCameraMoveEnds = new List<ObserverOfCameraMoveEnds>();

    public void AddObverserOfCameraMoveEnds(ObserverOfCameraMoveEnds observer)
    {
        _observerOfCameraMoveEnds.Add(observer);
    }

    public void RemoveObverserOfCameraMoveEnds(ObserverOfCameraMoveEnds observer)
    {
        _observerOfCameraMoveEnds.Remove(observer);
    }

    public void Notify()
    {
        foreach (ObserverOfCameraMoveEnds observer in _observerOfCameraMoveEnds)
        {
            observer.OnNotifyCameraMoveEnds();
        }
    }

    private void Start()
    {
        GameManager.MoveCamera += MoveCamera;
        
    }
    private void OnDestroy()
    {
        GameManager.MoveCamera -= MoveCamera;

    }

    private void MoveCamera(Vector3 p)
    {
        targetPos = p;
        isMoving = true;

    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, MoveSpeed * Time.deltaTime);

            if(Vector3.Distance(transform.position, targetPos) < .01f)
            {
                isMoving = false;

                // JT: as a subject be observered
                Notify();
                Debug.Log("camera move ends");
            }
        }
    }
}
