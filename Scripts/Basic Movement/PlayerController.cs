using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rgbd;
    float horizontalMove = 0f;
    public float runSpeed = 40f;
    private Vector3 m_Velocity = Vector3.zero;
    const float GroundedRadius = .2f; 
	private bool Grounded;            
	const float CeilingRadius = .2f; 
	private bool FacingRight = true;
    private bool WalkingBackward = false;
    public Animator animator;
    public Transform Gun;

    Vector2 direction;
    Quaternion rotation;

    [Range(0, .3f)] [SerializeField] private float MovementSmoothing = .05f;
    [Range(0, 1)] [SerializeField] private float CrouchSpeed = .36f;
    [SerializeField] private Transform GroundCheck;							
	[SerializeField] private Transform CeilingCheck;
    [SerializeField] private LayerMask WhatIsGround;
    [SerializeField] private float JumpForce = 400f;
    [SerializeField] private Collider2D CrouchDisableCollider;
    [SerializeField] private bool AirControl = false;
	[SerializeField] private float knockbackForce = 10f;
    


    [Header("Events")]
	[Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
    private bool wasCrouching = false;

    bool jump = false;
    bool crouch = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
	{ 
		rgbd = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Gun.transform.position;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        if (direction.x < 0 && FacingRight)
        {
            Flip();
        }
        if (direction.x > 0 && !FacingRight)
        {
            Flip();
        }

        if (Input.GetButtonDown("Jump")) {
            jump = true;
            animator.SetBool("IsJumping", true);
        }
        if (Input.GetButtonDown("Crouch")) {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch")) {
            crouch = false;
        }
        animator.SetBool("IsWalkingBackward", WalkingBackward);

        FaceMouse();
    }

    void FaceMouse()
    {
        Gun.transform.up = direction;
    }

    

    public void Move(float move, bool crouch, bool jump) {
        if (!crouch) {
			if (Physics2D.OverlapCircle(CeilingCheck.position, CeilingRadius, WhatIsGround)) {
				crouch = true;
			}
		}
		if (Grounded || AirControl) {
			if (crouch) {
				if (!wasCrouching) {
					wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}
				move *= CrouchSpeed;
				if (CrouchDisableCollider != null)
					CrouchDisableCollider.enabled = false;
			    } else {
				if (CrouchDisableCollider != null)
					CrouchDisableCollider.enabled = true;

				if (wasCrouching) {
					wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}
            Vector3 targetVelocity = new Vector2(move * 10f, rgbd.velocity.y);
            rgbd.velocity = Vector3.SmoothDamp(rgbd.velocity, targetVelocity, ref m_Velocity, MovementSmoothing);
            if (Grounded && jump) {
			    Grounded = true;
			    rgbd.AddForce(new Vector2(0f, JumpForce));
		    }
            if (move > 0 && !FacingRight) {
				WalkingBackward = true;
		    }
		    else if (move < 0 && FacingRight) {
			    WalkingBackward = true;
		    }
            else {
                WalkingBackward = false;
            }
        }
    }   

    void FixedUpdate () {
        Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;

        bool wasGrounded = Grounded;
		Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, GroundedRadius, WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
    }

    private void Flip()
	{
		FacingRight = !FacingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    public void OnLanding() {
        animator.SetBool("IsJumping", false);
    }

    public void OnCrouching(bool IsCrouching) {
        animator.SetBool("IsCrouching", IsCrouching);
    }
}


