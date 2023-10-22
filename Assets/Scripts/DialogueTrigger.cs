using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueHandler handler;

    public void TriggerDialogue()
    {
        handler.StartDialogue(dialogue);
    }
}
