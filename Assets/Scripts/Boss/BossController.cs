using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour {
	public enum BossMode {
		Easy,
		Medium,
		Hard,
		Inactive
	}

	[SerializeField] private GameObject _healthBar;
	[SerializeField] private BossMode _mode;
	[SerializeField] private SpawnManager[] _shootPoints;

	private bool _canTakeDamage = true;
	private int _damageTaken = 0;

	private void OnTriggerStay2D(Collider2D collision) {
		if (collision.gameObject.CompareTag("Player"))
			PushBack(collision.attachedRigidbody, collision.transform);
	}

	private void OnTriggerExit2D(Collider2D collision) => _canTakeDamage = true;

	private void Update() {
		if (_mode == BossMode.Inactive)
			foreach (SpawnManager shootPoint in _shootPoints)
				shootPoint.enabled = false;

		if (_mode == BossMode.Easy)
			_shootPoints[0].enabled = true;
		else if (_mode == BossMode.Medium)
			_shootPoints[1].enabled = true;
		else if (_mode == BossMode.Hard)
			_shootPoints[2].enabled = true;
	}

	private void PushBack(Rigidbody2D rigidbody, Transform collided) {
		rigidbody.AddForce(GetHitDirection(collided.transform) * 10f, ForceMode2D.Impulse);

		StopCollidedMovement(collided);

		if (_canTakeDamage)
			TakeDamage();
	}

	private void StopCollidedMovement(Transform collided) => collided.gameObject.GetComponent<PlayerController>().StopMovement();

	private Vector2 GetHitDirection(Transform collided) {
		return collided.position - transform.position;
	}

	private void TakeDamage() {
		if (_damageTaken >= 3)
			return;

		GameObject healthPoint = _healthBar.transform.GetChild(_damageTaken).gameObject;
		healthPoint.GetComponent<Image>().enabled = false;
		_damageTaken++;
		_mode++;

		_canTakeDamage = false;
	}
}
