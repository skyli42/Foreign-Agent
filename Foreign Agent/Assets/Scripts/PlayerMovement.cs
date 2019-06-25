using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
	public float turnSpeed = 20f;
	public float moveSpeed = 0.05f;
	private float originalSpeed;
	public float maxDash = 2f;
	private float dash = 2f;
	public float dashSpeed;

	public Slider slider;
	private bool dashStart = false;
	private float sliderTimer = 0;
	public float regenRate = 0.5f;
	Quaternion fixedRotation;


	Animator m_Animator;
	Rigidbody m_Rigidbody;
	AudioSource m_AudioSource;
	Vector3 m_Movement;
	Quaternion m_Rotation = Quaternion.identity;



	void Start()
	{
		m_Animator = GetComponent<Animator>();
		m_Rigidbody = GetComponent<Rigidbody>();
		m_AudioSource = GetComponent<AudioSource>();
		originalSpeed = moveSpeed;
		dash = maxDash;
		fixedRotation = slider.GetComponentInParent<Canvas>().transform.rotation;
	}

	void FixedUpdate()
	{
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		m_Movement.Set(horizontal, 0f, vertical);
		m_Movement.Normalize();

		bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
		bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
		bool isWalking = hasHorizontalInput || hasVerticalInput;
	
		m_Animator.SetBool("IsWalking", isWalking);
		
		Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
		m_Rotation = Quaternion.LookRotation(desiredForward);
	}

	void OnAnimatorMove()
	{
		Vector3 delPos = m_Movement * (m_Animator.deltaPosition.magnitude + moveSpeed);
		m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
		m_Rigidbody.MoveRotation(m_Rotation);
	}
	private void LateUpdate()
	{
		slider.GetComponentInParent<Canvas>().transform.rotation = fixedRotation;
	}
	private void Update()
	{
		if (slider.value == 1)
		{
			sliderTimer += Time.deltaTime;
			if (sliderTimer > 1)
			{
				slider.gameObject.SetActive(false);
			}
		}
		if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
		{
			slider.gameObject.SetActive(true);
			sliderTimer = 0;
			dashStart = true;
		}
		else if ((Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift)) || (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0))
		{
			moveSpeed = originalSpeed;
			dashStart = false;
		}
		if (dash <= 0)
		{
			dashStart = false;
		}
		if (dashStart)
		{
			dash -= Time.deltaTime;
			moveSpeed = dashSpeed;
			if (dash <= 0)
			{
				moveSpeed = originalSpeed;
			}
		}
		else
		{
			if (dash < maxDash)
			{
				dash += regenRate * Time.deltaTime;
				if (dash > maxDash)
				{
					dash = maxDash;
				}
			}
		}
        m_Animator.SetBool("IsRunning", dashStart);
        float complete = 1 - (maxDash - dash) / maxDash;
		slider.value = complete;
	}
}


