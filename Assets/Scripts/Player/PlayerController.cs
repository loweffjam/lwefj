using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
	[SerializeField] private float _speed = 250f;
	[SerializeField] private float _dashMultiplier = 10f;
	[SerializeField] private float _dashDuration = .05f;
	[SerializeField] private float _dashCooldown = .8f;

	private Rigidbody2D _rigidbody;
	private Animator _animator;
	private Vector2 _moveInput;
	private bool _canMove = true;
	private bool _canDash = true;
	private float _dashSpeed = 1f;

	private void Start() {
		_rigidbody = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
	}

	private void FixedUpdate() {
		if (_canMove)
			_rigidbody.velocity = _moveInput * _speed * _dashSpeed * Time.fixedDeltaTime;
	}

	private void Update() {
		if (!_canMove)
			StartCoroutine(WaitThen(1f, ResumeMovement));
	}

	public void StopMovement() => _canMove = false;

	public void Move(InputAction.CallbackContext context) {
		_moveInput = context.ReadValue<Vector2>();
		SetAnimationDirection();

		if (!_canMove)
			return;

		if (context.started || context.performed)
			_animator.SetBool("IsMoving", true);
		else if (context.canceled)
			_animator.SetBool("IsMoving", false);
	}

	private void SetAnimationDirection() {
		if (_moveInput.y < 0)
			_animator.SetInteger("Direction", 0);
		if (_moveInput.y > 0)
			_animator.SetInteger("Direction", 1);
		if (_moveInput.x > 0)
			_animator.SetInteger("Direction", 2);
		if (_moveInput.x < 0)
			_animator.SetInteger("Direction", 3);
	}

	public void Dash(InputAction.CallbackContext context) {
		if (_canDash && context.started) {
			_dashSpeed = _dashMultiplier;
			SetInvincibility(true);
			StartCoroutine(WaitThen(_dashDuration, ResetDashState));
		}
	}

	private IEnumerator WaitThen(float seconds, Action action) {
		yield return new WaitForSeconds(seconds);
		action();
	}

	private void ResumeMovement() => _canMove = true;

	private void ResetDashState() {
		_dashSpeed = 1f;
		_canDash = false;
		SetInvincibility(false);

		StartCoroutine(WaitThen(_dashCooldown, ResumeDash));
	}

	private void ResumeDash() => _canDash = true;

	private void SetInvincibility(bool state) => Physics2D.IgnoreLayerCollision(6, 9, state);
}
