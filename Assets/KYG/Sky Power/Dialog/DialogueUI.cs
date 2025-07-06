using KYG.SkyPower.Dialogue;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace KYG.SkyPower.Dialogue
{
    /// <summary>
    /// ���̾�α� UI�� �����ϴ� ������Ʈ�Դϴ�.
    /// </summary>
    /// <remarks>
    /// �� ������Ʈ�� DialogueManagerSO�� ����Ǿ� ���̾�α��� ���¿� ���� UI�� ������Ʈ�մϴ�.
    /// </remarks>

public class DialogueUI : MonoBehaviour
{
    public TMP_Text speakerText;
    public TMP_Text contentText;
    public GameObject rootPanel; // ���̾�α� UI ��ü

    void OnEnable()
    {
        DialogueManagerSO.Instance.OnLineUpdated.AddListener(UpdateUI);
        DialogueManagerSO.Instance.OnDialogueStarted.AddListener(OpenUI);
        DialogueManagerSO.Instance.OnDialogueEnded.AddListener(CloseUI);
    }

    void OnDisable()
    {
        DialogueManagerSO.Instance.OnLineUpdated.RemoveListener(UpdateUI);
        DialogueManagerSO.Instance.OnDialogueStarted.RemoveListener(OpenUI);
        DialogueManagerSO.Instance.OnDialogueEnded.RemoveListener(CloseUI);
    }

    void UpdateUI(string speaker, string content)
    {
        rootPanel.SetActive(true);
        speakerText.text = speaker;
        contentText.text = content;
    }
    void OpenUI() => rootPanel.SetActive(true);
    void CloseUI() => rootPanel.SetActive(false);

    public void OnNextButton() => DialogueManagerSO.Instance.NextLine();
}
}