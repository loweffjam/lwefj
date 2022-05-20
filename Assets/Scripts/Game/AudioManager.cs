using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public static event Action<Sound> OnDefineSoundPlayerFootsteps;
	public static event Action<Sound> OnDefineSoundPlayerDash;
	public static event Action<Sound> OnDefineSoundPlayerTookDamage;
	public static event Action<Sound> OnDefineSoundBossTookDamage;

	[Serializable]
	public enum Trigger {
		PlayerFootsteps,
		PlayerDash,
		PlayerTookDamage,
		BossTookDamage,
	}

	public List<Sound> sounds = new List<Sound>();
	public Trigger currentEdittingSound;
	[HideInInspector] public Trigger lastEdittingSound;

	private Dictionary<Trigger, Action<Sound>> _ActionsByTrigger = new Dictionary<Trigger, Action<Sound>> {
		{ Trigger.PlayerFootsteps, (sound) => OnDefineSoundPlayerFootsteps?.Invoke(sound) },
		{ Trigger.PlayerDash, (sound) => OnDefineSoundPlayerDash?.Invoke(sound) },
		{ Trigger.PlayerTookDamage, (sound) => OnDefineSoundPlayerTookDamage?.Invoke(sound) },
		{ Trigger.BossTookDamage, (sound) => OnDefineSoundBossTookDamage?.Invoke(sound) },
	};

	private void Start() {
		foreach (Sound sound in sounds)
			_ActionsByTrigger[sound.trigger](sound);
	}

	private void Update() {
		if (currentEdittingSound != lastEdittingSound)
			lastEdittingSound = currentEdittingSound;
	}

	public static void ChangeSound(out AudioClip audioClip, Sound sound) => audioClip = sound.audio;

	public static void PlaySoundOnce(AudioSource audioSource, AudioClip audioClip) => audioSource.PlayOneShot(audioClip);
}