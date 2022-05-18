using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {
	protected Action<Bullet> _killAction;

	private Rigidbody2D _rigidbody;

	private void Awake() => GetRigidbody();

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Walls") || collision.gameObject.CompareTag("Player"))
			_killAction(this);
	}

	public void AddForwardForce() {
		if (_rigidbody != null)
			_rigidbody.AddForce(transform.up * 10f, ForceMode2D.Impulse);
		else {
			GetRigidbody();
			AddForwardForce();
		}
	}

	public void Reset(Transform parent) {
		transform.position = parent.position;
		transform.rotation = parent.rotation;
	}

	public void SetKill(Action<Bullet> killAction) => _killAction = killAction;

	private void GetRigidbody() => _rigidbody = GetComponent<Rigidbody2D>();
}