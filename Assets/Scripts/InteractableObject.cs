using UnityEngine;
using UnityEngine.UI;


public class InteractableObject : MonoBehaviour
{
    public float interactionDistance = 2f; // popup distance

    public SpriteRenderer characterSprite;
    
    public Sprite firstSprite;
    public Sprite secondSprite;
    public Sprite thirdSprite;

    public GameObject popupText;
    public GameObject player;

    bool spokenTo = true;
    bool isImpressed = false;
    bool isBeat = false;

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
        if (isInRange && !isBeat && Input.GetKeyDown(KeyCode.E))
        {
            if (!spokenTo)
            {
                //trigger dialogue
                spokenTo = true;
            }
            else
            {
                StartMinigame();
            }
        }

    }

    private void StartMinigame()
    {
        if (gameObject.CompareTag("Fairy"))
        {
            GameManager.instance.MakeRoundOne();
        }
        if (gameObject.CompareTag("Vampire"))
        {
            GameManager.instance.MakeRoundTwo();
        }
        if (gameObject.CompareTag("Wizard"))
        {
            GameManager.instance.MakeRoundThree();
        }
    }
}

