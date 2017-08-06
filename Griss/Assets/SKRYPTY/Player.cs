using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private Rigidbody2D rb;
    private Animator myAnimator;
    public float speed;

    private bool facingRight;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    private bool isGrounded;

    private bool jump;

    [SerializeField]
    private float jumpForce;



	// Use this for initialization
	void Start () {

        facingRight = true;
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
	}

    void Update()
    {
        HandleInput();
    }

    // Update is called once per frame
    void FixedUpdate () {
        float horizontal = Input.GetAxis("Horizontal");
        Debug.Log(horizontal);

        isGrounded = IsGrounded();
        Debug.Log(isGrounded);

        HandleMovement(horizontal);

        Flip(horizontal);

        ResetValues();

        }


    private void HandleMovement(float horizontal)
    {

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        if (isGrounded && jump)
        {
            isGrounded = false;
            rb.AddForce(new Vector2(0, jumpForce));
        }

        myAnimator.SetFloat("speed", Mathf.Abs (horizontal));
        
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
    }


    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;

            transform.localScale = theScale;
        }
    }

    private void ResetValues()
    {
        jump = false;
    }

    private bool IsGrounded()
    {
        if (rb.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;

                    }
                }
            }
        }
        return false;
    }
}
