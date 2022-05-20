using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(AudioSource))]
public class AudioSourceCustomInspector : Editor {
	private AudioManager _audioManager;
	private AudioSource _emitterSource;
	private AudioSource _audioSourceRefGui;

	private void OnEnable() {
		AudioSource audioSource = target as AudioSource;
		try {
			_audioManager = audioSource.gameObject.GetComponent<AudioManager>();
		}
		catch { } // Intentionally blank: during initialization the gameObject is not created yet causing a Reference Exception, which is resolved later

		if (_audioManager != null) {
			foreach (Sound sound in _audioManager.sounds) {
				if (sound.trigger == _audioManager.currentEdittingSound)
					_emitterSource = sound.emitterObject.GetComponent<AudioSource>();
			}
		}
	}

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		_audioSourceRefGui = (AudioSource)EditorGUILayout.ObjectField("Audio Source Reference", _audioSourceRefGui, typeof(AudioSource), true);

		if (_emitterSource != null)
			_audioSourceRefGui = _emitterSource;

		if (GUI.changed && _audioSourceRefGui != null)
			_audioSourceRefGui = GameManager.CopyAudioSource((AudioSource)target, _audioSourceRefGui.gameObject);
	}
}
