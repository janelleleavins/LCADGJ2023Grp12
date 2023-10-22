using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public float interactionDistance = 2f; // popup distance
    public GameObject popupText;
    public GameObject player;
    private bool isInRange = false;

    private void Start()
    {
        popupText.SetActive(false);
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= interactionDistance)
        {
            isInRange = true;
            popupText.SetActive(true);
        }
        else
        {
            isInRange = false;
            popupText.SetActive(false);
        }

        //  E key is pressed
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            // call some function that does the thing here
            Debug.Log("Interacted with the object.");
        }
    }
}

