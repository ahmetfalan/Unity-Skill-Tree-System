using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private PlayerAttributes playerAttributes;

    public static PlayerControl Instance;

    //jump variables
    [Header("Ground Check")]
    public float distance = 0.6f;
    public LayerMask whatIsGround;

    //combonent variables
    private Rigidbody2D rb;


    private float boostSpeed;
    private float boostJump;

    private void Start()
    {
        Instance = this;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        //PlayerAttributes._points = PlayerPrefs.GetInt("Point");
        PlayerPrefs.DeleteAll();


        boostSpeed = playerAttributes.Speed;
        boostJump = playerAttributes.Jump;

        TooltipSystem.Hide();
        UpdateStats();
    }
    private void Update()
    {
        float x = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(x * playerAttributes.Speed, rb.velocity.y);
        if (Input.GetKeyDown("w") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, playerAttributes.Jump);
        }
    }

    private bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, whatIsGround);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    public void UpdateStats()
    {
        if (SkillTreeReader.Instance.IsSkillUnlocked(0)) {}
        if (SkillTreeReader.Instance.IsSkillUnlocked(1))
            playerAttributes.Speed = boostSpeed + 5;
        if (SkillTreeReader.Instance.IsSkillUnlocked(2))
            playerAttributes.Jump = boostJump + 4;
    }
}
