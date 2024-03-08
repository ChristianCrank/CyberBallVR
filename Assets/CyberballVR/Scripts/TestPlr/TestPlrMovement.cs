using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlrMovement : MonoBehaviour
{
	public Rigidbody playerRigid;
	public float w_speed, wb_speed, olw_speed, rn_speed, ro_speed;
	public bool walking;
	public Transform playerTrans;


	void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.W))
		{
			playerRigid.velocity = transform.forward * w_speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.S))
		{
			playerRigid.velocity = -transform.forward * wb_speed * Time.deltaTime;
		}
	}
	void Update()
	{
		//if (Input.GetKeyDown(KeyCode.P)) EventManager.onTogglePitch.Invoke();

		if (Input.GetKeyDown(KeyCode.W))
		{
			
			walking = true;
		}
		if (Input.GetKeyUp(KeyCode.W))
		{

		
			walking = false;
			w_speed = olw_speed;
		}
	
		if (Input.GetKeyUp(KeyCode.S))
		{
		
			walking = false;
		}
		if (Input.GetKey(KeyCode.A))
		{
			playerTrans.Rotate(0, -ro_speed * Time.deltaTime, 0);
		}
		if (Input.GetKey(KeyCode.D))
		{
			playerTrans.Rotate(0, ro_speed * Time.deltaTime, 0);
		}
		if (walking == true)
		{
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				w_speed = w_speed + rn_speed;
				
			}

			if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				w_speed = olw_speed;
			
			}

		}


	}
}
