using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	[Tooltip("The targeted fps limit")]
	[SerializeField] private int _targetFrameRate = 60;

	private void Awake() => LimitFrameRate();
	private void LimitFrameRate() {
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = _targetFrameRate;
	}
}
