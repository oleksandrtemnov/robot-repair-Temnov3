using UnityEngine;
public class NonPlayerCharacter : MonoBehaviour
{
    public GameObject dialogueBubble;
    void Start() { SetBubble(false); }
    public void SetBubble(bool visible) { if (dialogueBubble != null) dialogueBubble.SetActive(visible); }
}
