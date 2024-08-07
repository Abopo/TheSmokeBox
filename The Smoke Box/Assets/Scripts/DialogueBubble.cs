using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBubble : MonoBehaviour {

    [SerializeField]
    GameObject _speechBubble;

    SuperTextMesh _dialogueText;
    string[] _dialogueToShow;
    int _dialogueIndex = 0;

    Competitor _competitor;

    private void Awake() {
        _dialogueText = GetComponentInChildren<SuperTextMesh>(true);
        _competitor = GetComponentInParent<Competitor>();
    }
    // Start is called before the first frame update
    void Start() {
        _dialogueText.OnCompleteEvent += OnDialogueComplete;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ShowDialogue(string dialogue) {
        if(dialogue.Contains("[Submission]")) {
            // Replace it with the submission name
            dialogue = dialogue.Replace("[Submission]", _competitor.SubmissionTitle);
        }

        _dialogueText.text = dialogue;
        _speechBubble.SetActive(true);
    }

    public void ShowDialogues(string[] dialogue) {
        _dialogueToShow = dialogue;
        _dialogueText.text = _dialogueToShow[_dialogueIndex++];
        _speechBubble.SetActive(true);
    }

    void OnDialogueComplete() {
        if (_dialogueToShow != null && _dialogueIndex < _dialogueToShow.Length) {
            StartCoroutine(NextDialogueLater());
        } else {
            StartCoroutine(HideDialogueLater());
            // Clear the dialogue array
            _dialogueToShow = null;
        }
    }

    IEnumerator NextDialogueLater() {
        yield return new WaitForSeconds(1f);

        ShowDialogue(_dialogueToShow[_dialogueIndex++]);
    }

    IEnumerator HideDialogueLater() {
        yield return new WaitForSeconds(2f);

        // Double check that we're not reading text
        if (!_dialogueText.reading) {
            _speechBubble.SetActive(false);
        }
    }
}
