using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
	[SerializeField] private GameObject _sfxAudioSourceObj;
	[SerializeField] private GameObject _footstepsAudioSourceObj;
	[SerializeField] private float _speed = 250f;
	[SerializeField] private float _dashMultiplier = 10f;
	[SerializeField] private float _dashDuration = .05f;
	[SerializeField] private float _dashCooldown = .8f;

	private Rigidbody2D _rigidbody;
	private Animator _animator;
	private Vector2 _moveInput;
	private bool _canMove = true;
	private bool _canDash = true;
	private bool _startedDash = false;
	private bool _playerDashed = false;
	private float _dashSpeed = 1f;

	private AudioClip _footstepsSound; 
	private AudioClip _dashSound;
	private AudioClip _damageSound;

	private void OnEnable() {
		AudioManager.OnDefineSoundPlayerFootsteps += (sound) => AudioManager.ChangeSound(out _footstepsSound, sound);
		AudioManager.OnDefineSoundPlayerDash += (sound) => AudioManager.ChangeSound(out _dashSound, sound);
		AudioManager.OnDefineSoundPlayerTookDamage += (sound) => AudioManager.ChangeSound(out _damageSound, sound);
	}

	private void OnDisable() {
		AudioManager.OnDefineSoundPlayerFootsteps -= (sound) => AudioManager.ChangeSound(out _footstepsSound, sound);
		AudioManager.OnDefineSoundPlayerDash -= (sound) => AudioManager.ChangeSound(out _dashSound, sound);
		AudioManager.OnDefineSoundPlayerTookDamage -= (sound) => AudioManager.ChangeSound(out _damageSound, sound);
	}

	private void Start() {
		_rigidbody = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();
	}

	private void FixedUpdate() {
		if (_canMove)
			_rigidbody.velocity = _moveInput * _speed * _dashSpeed * Time.fixedDeltaTime;

		if (_startedDash) {
			_playerDashed = _moveInput != Vector2.zero && _dashSpeed != 1f;
			_startedDash = false;
		}
	}

	private void Update() {
		if (!_canMove)
			StartCoroutine(WaitThen(1f, ResumeMovement));

		if (_playerDashed) {
			AudioManager.PlaySoundOnce(_sfxAudioSourceObj.GetComponent<AudioSource>(), _dashSound);
			_playerDashed = false;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Bullet"))
			AudioManager.PlaySoundOnce(_sfxAudioSourceObj.GetComponent<AudioSource>(), _damageSound);
	}

	public void StopMovement() => _canMove = false;

	public void Move(InputAction.CallbackContext context) {
		_moveInput = context.ReadValue<Vector2>();
		SetAnimationDirection();

		if (!_canMove)
			return;

		if (context.started || context.performed) {
			_animator.SetBool("IsMoving", true);
			PlayFootsteps();
		}
		else if (context.canceled) {
			_animator.SetBool("IsMoving", false);
			PlayFootsteps(false);
		}
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
			_startedDash = true;

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
		SetInvincibility(false);

		_canDash = false;
		StartCoroutine(WaitThen(_dashCooldown, ResumeDash));
	}

	private void ResumeDash() => _canDash = true;

	private void SetInvincibility(bool state) => Physics2D.IgnoreLayerCollision(6, 9, state);

	private void PlayFootsteps(bool shouldPlay = true) {
		AudioSource footstepsAudioSource = _footstepsAudioSourceObj.GetComponent<AudioSource>();
		footstepsAudioSource.clip = _footstepsSound;

		if (shouldPlay && !footstepsAudioSource.isPlaying)
			footstepsAudioSource.Play();
		if (!shouldPlay)
			footstepsAudioSource.Stop();
	}
}
