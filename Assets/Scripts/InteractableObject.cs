using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public float interactionDistance = 2f; // popup distance
    public GameObject popupText;
    public GameObject player;
    private bool isInRange = false;

    public GameManager simonSays;

    public Sprite baseSprite;
    public Sprite impressedSprite;
    public Sprite beatSprite;

    public bool hasSpoken = false;
    public bool isImpressed = false;
    public bool isBeat = false;

    private void Start()
    {
        popupText.SetActive(false);
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= interactionDistance && !isBeat)
        {
            isInRange = true;
            popupText.SetActive(true);
        }
        else
        {
            isInRange = false;
            popupText.SetActive(false);
        }

        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!hasSpoken)
            {
                GetComponent<DialogueTrigger>().TriggerDialogue();
                PlayerMovement.instance.freezePlayer();
                hasSpoken = true;
            }
            else if (hasSpoken && !isBeat)
            {
                StartMinigame();
                PlayerMovement.instance.freezePlayer();
            }
        }
    }

    private void StartMinigame()
    {
        if (gameObject.CompareTag("Fairy"))
        {
            simonSays.MakeRoundOne();
        }
        if (gameObject.CompareTag("Vampire"))
        {
            simonSays.MakeRoundTwo();
        }
        if (gameObject.CompareTag("Wizard"))
        {
            simonSays.MakeRoundThree();
        }
    }

    public void impressSprite()
    {
        GetComponent<SpriteRenderer>().sprite = impressedSprite;
    }
    public void defeatSprite()
    {
        GetComponent<SpriteRenderer>().sprite = beatSprite;
        isBeat = true;
    }
}

