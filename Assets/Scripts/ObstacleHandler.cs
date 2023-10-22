using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHandler : MonoBehaviour
{
    public float interactionDistance = 5f; // popup distance
    public GameObject popupText;
    public ParticleSystem fireParticles;
    public GameObject player;

    public Sprite destroyedSprite;

    private bool isInRange = false;
    private bool interactedWith = false;
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

        if (isInRange && !interactedWith && PlayerMovement.instance.hasHorns && Input.GetKeyDown(KeyCode.E))
        {
            interactedWith = true;
            PlayerMovement.instance.freezePlayer();
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        //yield return new WaitForSeconds(1f);

        player.GetComponent<PlayerMovement>().attackSprites();
        fireParticles.Play();

        yield return new WaitForSeconds(2f);

        gameObject.GetComponent<BoxCollider>().enabled = false;

        yield return new WaitForSeconds(1f);

        fireParticles.Stop();
        PlayerMovement.instance.unfreezePlayer();

        player.GetComponent<PlayerMovement>().attackSprites();
        gameObject.GetComponent<SpriteRenderer>().sprite = destroyedSprite;
        yield return null;
    }
}
