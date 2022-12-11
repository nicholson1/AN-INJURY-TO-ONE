using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHazard : MonoBehaviour
{
    [SerializeField] private Sprite ButtonUp;
    [SerializeField] private Sprite ButtonDown;
    [SerializeField] private SpriteRenderer image;

    [SerializeField] private GameObject itsHazard;

    private HazardDisableOnEvent hazardDisable;
    public bool StopsOnButtonUp;

    // Start is called before the first frame update
    void Start()
    {
        hazardDisable = gameObject.GetComponentInChildren<HazardDisableOnEvent>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        PushHazardButton();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        LeaveHazardButton();
    }

    private void PushHazardButton()
    {

        //if (hazardDisable != null)
        //{
        //    hazardDisable.turnOff = true;
        //}

        itsHazard.SetActive(false);

        image.sprite = ButtonDown;
        GetComponent<Interactable>().isInteractable = false;
    }

    private void LeaveHazardButton()
    {
        image.sprite = ButtonUp;
        GetComponent<Interactable>().isInteractable = true;

        if (hazardDisable != null && StopsOnButtonUp)
        {
            hazardDisable.turnOff = false;
        }

        itsHazard.SetActive(true);
    }
}
