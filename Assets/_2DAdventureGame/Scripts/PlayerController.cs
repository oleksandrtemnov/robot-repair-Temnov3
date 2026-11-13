using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;
    public InputAction LaunchAction;
    public InputAction TalkAction;
    public float speed = 3f;
    public int maxHealth = 5;
    public float timeInvincible = 1.5f;
    public float shotCooldown = .45f;
    public float projectileSpeed = 6f;
    public GameObject projectilePrefab;
    public int health { get; private set; }
    public int hitsTaken { get; private set; }
    public Vector2 facing { get; private set; } = Vector2.right;
    Rigidbody2D body;
    Animator animator;
    SpriteRenderer visual;
    Vector2 move;
    float immunity, nextShot;
    NonPlayerCharacter lastNpc;
    void Awake()
    {
        body = GetComponent<Rigidbody2D>(); animator = GetComponent<Animator>();
        visual = GetComponent<SpriteRenderer>(); health = maxHealth;
        if (MoveAction == null || MoveAction.bindings.Count == 0)
        {
            MoveAction = new InputAction("Move", InputActionType.Value);
            MoveAction.AddCompositeBinding("2DVector").With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s").With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");
        }
        if (LaunchAction == null) LaunchAction = new InputAction("Launch", InputActionType.Button);
        if (TalkAction == null) TalkAction = new InputAction("Talk", InputActionType.Button);
        if (LaunchAction.bindings.Count == 0) LaunchAction.AddBinding("<Keyboard>/c");
        if (TalkAction.bindings.Count == 0) TalkAction.AddBinding("<Keyboard>/x");
    }
    void OnEnable() { MoveAction?.Enable(); LaunchAction?.Enable(); TalkAction?.Enable(); }
    void OnDisable() { MoveAction?.Disable(); LaunchAction?.Disable(); TalkAction?.Disable(); if(lastNpc != null) lastNpc.SetBubble(false); }
    void Start() { UIHandler.instance?.SetHealthValue(health / (float)maxHealth); }
    void Update()
    {
        if (health <= 0 || (GameManager.instance != null && GameManager.instance.ended)) { move = Vector2.zero; return; }
        immunity = Mathf.Max(0, immunity - Time.deltaTime);
        move = Vector2.ClampMagnitude(MoveAction.ReadValue<Vector2>(), 1);
        if (move.sqrMagnitude > .01f) facing = move.normalized;
        if (animator != null) { animator.SetFloat("Move X", facing.x); animator.SetFloat("Move Y", facing.y); animator.SetFloat("Speed", move.magnitude); }
        if (visual != null) visual.color = immunity > 0 && Mathf.Sin(Time.time * 30) > 0 ? new Color(1,.65f,.65f) : Color.white;
        if (LaunchAction.WasPressedThisFrame()) Launch();
        var hit = Physics2D.Raycast(body.position + Vector2.up * .2f, facing, 1.5f, LayerMask.GetMask("NPC"));
        var npc = hit.collider != null ? hit.collider.GetComponent<NonPlayerCharacter>() : null;
        if (lastNpc != null && lastNpc != npc) lastNpc.SetBubble(false);
        lastNpc = npc;
        if (npc != null) { npc.SetBubble(true); if (TalkAction.WasPressedThisFrame()) UIHandler.instance?.DisplayDialogue(); }
    }
    void FixedUpdate() { body.MovePosition(body.position + move * speed * Time.fixedDeltaTime); }
    public void ChangeHealth(int amount)
    {
        if (health <= 0 || (GameManager.instance != null && GameManager.instance.ended)) return;
        if (amount < 0) { if (immunity > 0) return; immunity = timeInvincible; hitsTaken++; }
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        UIHandler.instance?.SetHealthValue(health / (float)maxHealth);
    }
    public void Launch()
    {
        if (Time.time < nextShot || projectilePrefab == null || health <= 0) return;
        nextShot = Time.time + shotCooldown;
        var shot = Instantiate(projectilePrefab, body.position + facing * .65f + Vector2.up * .2f, Quaternion.identity);
        shot.GetComponent<Projectile>().Launch(facing, projectileSpeed);
        if (animator != null) animator.SetTrigger("Launch");
    }
}
