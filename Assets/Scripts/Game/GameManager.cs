using System;
using System.Reflection;
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

	public static T CopyComponent<T>(T original, GameObject destination) where T : Component {
		Type type = original.GetType();

		Component existing = destination.GetComponent(type);
		if (existing != null)
			DestroyImmediate(existing);

		Component copy = destination.AddComponent(type);
		BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
		PropertyInfo[] properties = type.GetProperties(flags);
		FieldInfo[] fields = type.GetFields(flags);
		foreach (PropertyInfo property in properties) {
			if (property.Name == "name")
				continue;
			try {
				property.SetValue(copy, property.GetValue(original, null), null);
			}
			catch { }
		}
		foreach (FieldInfo field in fields)
			field.SetValue(copy, field.GetValue(original));

		return copy as T;
	}

	public static AudioSource CopyAudioSource(AudioSource original, GameObject destination, bool enabled = true) {
		Type type = typeof(AudioSource);

		AudioSource existing = destination.GetComponent<AudioSource>();
		if (existing != null)
			DestroyImmediate(existing);

		AnimationCurve rolloff = original.GetCustomCurve(AudioSourceCurveType.CustomRolloff);
		AnimationCurve panLvl = original.GetCustomCurve(AudioSourceCurveType.SpatialBlend);
		AnimationCurve spread = original.GetCustomCurve(AudioSourceCurveType.Spread);
		AnimationCurve reverb = original.GetCustomCurve(AudioSourceCurveType.ReverbZoneMix);

		AudioSource copy = destination.AddComponent<AudioSource>();
		BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
		PropertyInfo[] properties = type.GetProperties(flags);
		FieldInfo[] fields = type.GetFields(flags);
		foreach (PropertyInfo property in properties) {
			if (property.Name == "minVolume" || property.Name == "maxVolume" || property.Name == "rolloffFactor" || property.Name == "name")
				continue;
			try {
				property.SetValue(copy, property.GetValue(original, null), null);
			}
			catch { }
		}
		foreach (FieldInfo field in fields)
			field.SetValue(copy, field.GetValue(original));

		destination.GetComponent<AudioSource>().SetCustomCurve(AudioSourceCurveType.CustomRolloff, rolloff);
		destination.GetComponent<AudioSource>().SetCustomCurve(AudioSourceCurveType.SpatialBlend, panLvl);
		destination.GetComponent<AudioSource>().SetCustomCurve(AudioSourceCurveType.Spread, spread);
		destination.GetComponent<AudioSource>().SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, reverb);

		destination.GetComponent<AudioSource>().enabled = enabled;

		return copy;
	}
}
