using KYG.SkyPower.Dialogue;
using KYG_skyPower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



namespace KYG.SkyPower.Dialogue
{
    // DialogueManagerSO: ��� ���� �̱���
    // - ��� ������, ���� �������� ��� �ε���, ��� ��� ���� ����
    // - UIManager, GameManager ��� ���� ����
    // - ��� ����, ����, ���� ��� ������Ʈ �̺�Ʈ ����

[CreateAssetMenu(fileName = "DialogueManagerSO", menuName = "Manager/DialogueManager")]
public class DialogueManagerSO : SOSingleton<DialogueManagerSO>
{
    [Header("������ ��� ������")]
    public DialogueDataSO currentDialogue;
    public int currentLineIndex;
    public bool isDialoguePlaying;

    // UIManager, GameManager ��� ���� ����
    public UnityEvent<string, string> OnLineUpdated = new UnityEvent<string, string>();
    public UnityEvent OnDialogueStarted = new UnityEvent();
    public UnityEvent OnDialogueEnded = new UnityEvent();

    public void StartDialogue(DialogueDataSO dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        isDialoguePlaying = true;
        OnDialogueStarted?.Invoke();
        ShowCurrentLine();
    }

    public void ShowCurrentLine()
    {
        if (!currentDialogue || currentLineIndex >= currentDialogue.lines.Count)
        {
            EndDialogue();
            return;
        }
        var line = currentDialogue.lines[currentLineIndex];
        OnLineUpdated?.Invoke(line.speaker, line.content);
    }

    public void NextLine()
    {
        if (!isDialoguePlaying) return;
        currentLineIndex++;
        ShowCurrentLine();
    }

    public void EndDialogue()
    {
        isDialoguePlaying = false;
        OnDialogueEnded?.Invoke();
    }
}
}