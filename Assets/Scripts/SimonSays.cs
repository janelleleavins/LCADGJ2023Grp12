using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SimonSays : MonoBehaviour
{


    public GameObject gameButtonPrefab;
    public List<ButtonSetting> buttonSettings;
    public Transform gameFieldPanelTransform;

    List<GameObject> gameButtons;
    int promptCount = 4;

    List<int> prompts;
    List<int> playerInputs;

    System.Random rg;

    bool inputEnabled = false;
    int completedRounds = 0;
    bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        //TODO: change the number of buttons dependent on the level. check for progress then update?
        gameButtons = new List<GameObject>();
        CreateGameButton(0, new Vector3(-66, 66));
        CreateGameButton(1, new Vector3(66, 66));
        CreateGameButton(2, new Vector3(-66, -66));
        CreateGameButton(3, new Vector3(66, -66));
        //if stage 2:
        //CreateGameButton(4, new Vector3(-200, 0));
        //CreateGameButton(5, new Vector3(200, 0));
        //if stage 3:
        //CreateGameButton(6, new Vector3(0, -200));
        //CreateGameButton(7, new Vector3(0, 200));

        StartCoroutine(MiniGame());
    }

    IEnumerator MiniGame()
    {
        inputEnabled = false;

        rg = new System.Random("test".GetHashCode());

        SetPrompts();

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < prompts.Count; i++)
        {
            Prompt(prompts[i]);

            yield return new WaitForSeconds(0.6f);
        }

        inputEnabled = true;
        yield return null;
    }

    void Prompt(int index)
    {
        LeanTween.value(gameButtons[index], buttonSettings[index].normalColor, buttonSettings[index].highlightColor, 0.25f).setOnUpdate((Color color) => {
            gameButtons[index].GetComponent<Image>().color = color;
        });

        LeanTween.value(gameButtons[index], buttonSettings[index].highlightColor, buttonSettings[index].normalColor, 0.25f)
            .setDelay(0.5f)
            .setOnUpdate((Color color) => {
                gameButtons[index].GetComponent<Image>().color = color;
            });
    }


    void SetPrompts()
    {
        prompts = new List<int>();
        playerInputs = new List<int>();

        for (int i = 0; i < promptCount; i++)
        {
            prompts.Add(rg.Next(0, gameButtons.Count));
        }
    }

    void CreateGameButton(int index, Vector3 position)
    {
        GameObject gameButton = Instantiate(gameButtonPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        gameButton.transform.SetParent(gameFieldPanelTransform);
        gameButton.transform.localPosition = position;

        gameButton.GetComponent<Image>().color = buttonSettings[index].normalColor;
        gameButton.GetComponent<Button>().onClick.AddListener(() => {
            OnGameButtonClick(index);
        });

        gameButtons.Add(gameButton);
    }

    // Update is called once per frame
    
    void OnGameButtonClick(int index)
    {
        if (!inputEnabled)
        {
            return;
        }

        Prompt(index);

        playerInputs.Add(index);

        if (prompts[playerInputs.Count - 1] != index)
        {
            Debug.Log("Try again!"); //TODO: Show player they messed up
            StartCoroutine(MiniGame());
        }
        if(prompts.Count == playerInputs.Count)
        {
            completedRounds++;
            if (completedRounds < 3)
            {
                promptCount++;
                StartCoroutine(MiniGame());
            }
            else
            {
                GameOver();
            }
            
        }
    }

    void GameOver()
    {
        gameOver = true; //TODO: Close Simon Says UI
        inputEnabled = false;
        completedRounds = 0;
    }
}
