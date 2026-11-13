using UnityEngine;
using UnityEngine.UIElements;
public class UIHandler : MonoBehaviour
{
    public static UIHandler instance { get; private set; }
    public float displayTime = 4f;
    public Sprite healthFrame, portrait, healthFill, dialogueFrame, winImage, loseImage;
    VisualElement bar, dialogue, win, lose;
    Label progress, hint;
    float dialogueTimer, hintTimer;
    void Awake() { instance = this; }
    void OnEnable()
    {
        instance = this;
        var root = GetComponent<UIDocument>().rootVisualElement;
        bar = root.Q<VisualElement>("HealthBar"); dialogue = root.Q<VisualElement>("NPCDialogue");
        win = root.Q<VisualElement>("WinScreenContainer"); lose = root.Q<VisualElement>("LoseScreenContainer");
        progress = root.Q<Label>("RepairCount"); hint = root.Q<Label>("Hint");
        if(hint != null) hint.style.display = DisplayStyle.None;
        SetImage(root.Q<VisualElement>("HealthBarBackground"), healthFrame);
        SetImage(root.Q<VisualElement>("CharacterPortrait"), portrait); SetImage(bar, healthFill);
        SetImage(dialogue, dialogueFrame); SetImage(root.Q<VisualElement>("WinScreen"), winImage); SetImage(root.Q<VisualElement>("LoseScreen"), loseImage);
        dialogue.style.display = DisplayStyle.None; win.style.display = DisplayStyle.None; lose.style.display = DisplayStyle.None;
    }
    void SetImage(VisualElement item, Sprite sprite) { if(item != null && sprite != null) item.style.backgroundImage = new StyleBackground(sprite); }
    public void SetHealthValue(float percentage) { if(bar != null) bar.style.width = Length.Percent(Mathf.Clamp01(percentage) * 100); }
    public void SetProgress(int value, int total) { if(progress != null) progress.text = "Відремонтовано " + value + " / " + total; }
    public void DisplayDialogue() { dialogue.style.display = DisplayStyle.Flex; dialogueTimer = displayTime; }
    public void ShowHint(string message) { if(hint != null) { hint.text = message; hint.style.display = DisplayStyle.Flex; } hintTimer = 2.5f; }
    void Update()
    {
        if(dialogueTimer > 0 && (dialogueTimer -= Time.deltaTime) <= 0) dialogue.style.display = DisplayStyle.None;
        if(hintTimer > 0 && (hintTimer -= Time.deltaTime) <= 0 && hint != null) { hint.text = ""; hint.style.display = DisplayStyle.None; }
    }
    public void DisplayWinScreen() { ShowEnd(win); }
    public void DisplayLoseScreen() { ShowEnd(lose); }
    void ShowEnd(VisualElement item) { dialogue.style.display = DisplayStyle.None; item.style.display = DisplayStyle.Flex; item.style.opacity = 0; item.schedule.Execute(()=>item.style.opacity = 1).ExecuteLater(20); }
    void OnDestroy() { if(instance == this) instance = null; }
}
