using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour
{
    //public Button goNext;
    //public TextMeshProUGUI text;
    private Queue<string> dialogue;

    private void Start()
    {
        dialogue = new Queue<string>();
    }

}
