using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManager : MonoBehaviour {
	[Tooltip("The Bullet prefab")]
	[SerializeField] private Bullet _bulletPrefab;

	[Tooltip("A delay time in seconds to wait before start spawning")]
	[SerializeField] private float _startDelay = 2;

	[Tooltip("The time in seconds in which the spawn will keep repeating")]
	[SerializeField] private float _repeatRate = 2;

	private ObjectPool<Bullet> _pool;
	private float _timer = 0f;
	private int _bulletCount = 0;

	private void Start() {
		_pool = new ObjectPool<Bullet>(CreateBullet, OnGetBulletFromPool, OnReleaseBulletToPool, Bullet => Destroy(Bullet.gameObject), true, 6, 12);
		_timer = _startDelay;
	}

	private void Update() {
		if (/* condition is */ true) {
			_timer -= Time.deltaTime;
			if (_timer < 0) {
				_timer = _repeatRate;
				SpawnBullet();
			}
		}
	}

	private Bullet CreateBullet() {
		Bullet bullet = Instantiate(_bulletPrefab, transform.position, transform.rotation, transform);

		bullet.name = "Bullet (" + _bulletCount + ")";
		_bulletCount++;

		return bullet;
	}

	private void OnGetBulletFromPool(Bullet bullet) {
		bullet.gameObject.SetActive(true);
		bullet.AddForwardForce();
	}

	private void OnReleaseBulletToPool(Bullet bullet) {
		bullet.Reset(transform);
		bullet.gameObject.SetActive(false);
	}

	private void SpawnBullet() {
		Bullet bullet = _pool.Get();
		bullet.GetComponent<Bullet>().SetKill(Kill);
	}

	private void Kill(Bullet bullet) => _pool.Release(bullet);
}