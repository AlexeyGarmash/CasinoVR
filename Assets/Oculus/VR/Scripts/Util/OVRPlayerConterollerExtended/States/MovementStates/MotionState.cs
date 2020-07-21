using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class MotionState : MovementState
{
    public MotionState(OVRPlayerController context) : base(context)
    {
        
    }
    public override void Movement()
    {
		
		bool moveForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
		bool moveLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
		bool moveRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
		bool moveBack = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

		bool dpad_move = false;

			
		if (OVRInput.Get(OVRInput.Button.DpadUp))
		{
			moveForward = true;
			dpad_move = true;

		}

		if (OVRInput.Get(OVRInput.Button.DpadDown))
		{
			moveBack = true;
			dpad_move = true;
		}

		_context.MoveScale = 1.0f;

		if ((moveForward && moveLeft) || (moveForward && moveRight) ||
			(moveBack && moveLeft) || (moveBack && moveRight))
			_context.MoveScale = 0.70710678f;

		// No positional movement if we are in the air
		if (!_context.Controller.isGrounded)
			_context.MoveScale = 0.0f;

		_context.MoveScale *= _context.SimulationRate * Time.deltaTime;

		// Compute this for key movement
		float moveInfluence = _context.Acceleration * 0.1f * _context.MoveScale * _context.MoveScaleMultiplier;

		// Run!
		if (dpad_move || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			moveInfluence *= 2.0f;

		Quaternion ort = _context.transform.rotation;
		Vector3 ortEuler = ort.eulerAngles;
		ortEuler.z = ortEuler.x = 0f;
		ort = Quaternion.Euler(ortEuler);

		if (moveForward)
			_context.MoveThrottle += ort * (_context.transform.lossyScale.z * moveInfluence * Vector3.forward);
		if (moveBack)
			_context.MoveThrottle += ort * (_context.transform.lossyScale.z * moveInfluence * _context.BackAndSideDampen * Vector3.back);
		if (moveLeft)
			_context.MoveThrottle += ort * (_context.transform.lossyScale.x * moveInfluence * _context.BackAndSideDampen * Vector3.left);
		if (moveRight)
			_context.MoveThrottle += ort * (_context.transform.lossyScale.x * moveInfluence * _context.BackAndSideDampen * Vector3.right);



		moveInfluence = _context.Acceleration * 0.1f * _context.MoveScale * _context.MoveScaleMultiplier;

#if !UNITY_ANDROID // LeftTrigger not avail on Android game pad
		moveInfluence *= 1.0f + OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
#endif

		Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

		// If speed quantization is enabled, adjust the input to the number of fixed speed steps.
		if (_context.FixedSpeedSteps > 0)
		{
			primaryAxis.y = Mathf.Round(primaryAxis.y * _context.FixedSpeedSteps) / _context.FixedSpeedSteps;
			primaryAxis.x = Mathf.Round(primaryAxis.x * _context.FixedSpeedSteps) / _context.FixedSpeedSteps;
		}

		if (primaryAxis.y > 0.0f)
			_context.MoveThrottle += ort * (primaryAxis.y * _context.transform.lossyScale.z * moveInfluence * Vector3.forward);

		if (primaryAxis.y < 0.0f)
			_context.MoveThrottle += ort * (Mathf.Abs(primaryAxis.y) * _context.transform.lossyScale.z * moveInfluence *
									_context.BackAndSideDampen * Vector3.back);

		if (primaryAxis.x < 0.0f)
			_context.MoveThrottle += ort * (Mathf.Abs(primaryAxis.x) * _context.transform.lossyScale.x * moveInfluence *
									_context.BackAndSideDampen * Vector3.left);

		if (primaryAxis.x > 0.0f)
			_context.MoveThrottle += ort * (primaryAxis.x * _context.transform.lossyScale.x * moveInfluence * _context.BackAndSideDampen *
									Vector3.right);
    }
}

