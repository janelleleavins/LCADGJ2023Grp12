using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject gameButtonPrefab;
    public List<ButtonSetting> buttonSettings;
    public Transform gameFieldPanelTransform;
    public GameObject playerModel;
    public GameObject neighborModel1;
    public GameObject neighborModel2;
    public GameObject neighborModel3;

    public Animator animator;
    public GameObject textBox;

    List<GameObject> gameButtons;
    int promptCount; //how many it asks at first
    GameObject opponent;

    List<int> prompts;
    List<int> playerInputs;

    System.Random rg;

    bool inputEnabled = false;
    int completedRounds = 0;

    private void Start()
    {
        animator.GameObject().SetActive(false);
        //textBox.SetActive(false);

    }

    private void Awake()
    {
        instance = null;
    }

    public void MakeRoundOne()
    {
        gameButtons = new List<GameObject>();
        opponent = neighborModel1;
        promptCount = 4;
        CreateGameButton(0, new Vector3(-66, 66));
        CreateGameButton(1, new Vector3(66, 66));
        CreateGameButton(2, new Vector3(-66, -66));
        CreateGameButton(3, new Vector3(66, -66));
        StartCoroutine(MiniGame());
    }

    public void MakeRoundTwo()
    {
        gameButtons = new List<GameObject>();
        opponent = neighborModel2;
        promptCount = 5;
        CreateGameButton(0, new Vector3(-66, 66));
        CreateGameButton(1, new Vector3(66, 66));
        CreateGameButton(2, new Vector3(-66, -66));
        CreateGameButton(3, new Vector3(66, -66));
        CreateGameButton(4, new Vector3(-200, 0));
        CreateGameButton(5, new Vector3(200, 0));
        StartCoroutine(MiniGame());
    }

    public void MakeRoundThree()
    {
        gameButtons = new List<GameObject>();
        opponent = neighborModel3;
        promptCount = 6;
        CreateGameButton(0, new Vector3(-66, 66));
        CreateGameButton(1, new Vector3(66, 66));
        CreateGameButton(2, new Vector3(-66, -66));
        CreateGameButton(3, new Vector3(66, -66));
        CreateGameButton(4, new Vector3(-200, 0));
        CreateGameButton(5, new Vector3(200, 0));
        CreateGameButton(6, new Vector3(0, -200));
        CreateGameButton(7, new Vector3(0, 200));
        StartCoroutine(MiniGame());
    }

    IEnumerator MiniGame()
    {
        inputEnabled = false;

        rg = new System.Random();

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
            Encourage();
            StartCoroutine(MiniGame());
        }
        if(prompts.Count == playerInputs.Count)
        {
            completedRounds++;
            if (completedRounds < 3)
            {
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
        //"closes" Simon Says UI
        for( int i = 0; i < gameButtons.Count; i++ )
        {
            Destroy(gameButtons[i]);
        }

        PlayerMovement.instance.unfreezePlayer();
        Destroy(opponent);

        Celebrate();

        inputEnabled = false;
        completedRounds = 0;
    }

    void Encourage()
    {
        animator.gameObject.SetActive(true);
        animator.SetTrigger("Bounce");
        Debug.Log("Try again!");
    }
    void Celebrate()
    {
        //TODO: tell player they did a good job :3
        //show opponent going back inside or something??
        Debug.Log("Good job!");
        
    }
}
