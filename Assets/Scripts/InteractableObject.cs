using System.ComponentModel;
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

    public Sprite farewell;

    public GameObject victoryMessage;
    public GameObject treatPrize;

    public bool hasSpoken = false;
    public bool isImpressed = false;
    public bool isBeat = false;
    public bool hasPrize = false;

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
            else if (hasSpoken && isBeat && hasPrize)
            {
                if (gameObject.CompareTag("Fairy"))
                {
                    PlayerMovement.instance.giveHorns();
                }
                else if (gameObject.CompareTag("Vampire"))
                {
                    PlayerMovement.instance.giveWings();
                }
                else if (gameObject.CompareTag("Wizard"))
                {
                    //TODO: special something! idk! animation for bone ???
                }
                else
                {
                    Debug.Log("fix opponent tags");
                }

                treatPrize.SetActive(false);
                victoryMessage.GetComponent<SpriteRenderer>().sprite = farewell;
                hasPrize = false;
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

        victoryMessage.GetComponent<SpriteRenderer>().enabled = true;
        treatPrize.SetActive(true);

        isBeat = true;
        hasPrize = true;
    }
}

