using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public static event Action<Sound> OnDefineSoundPlayerFootsteps;
	public static event Action<Sound> OnDefineSoundPlayerDash;
	public static event Action<Sound> OnDefineSoundPlayerTookDamage;
	public static event Action<Sound> OnDefineSoundBossTookDamage;

	public enum Trigger {
		PlayerFootsteps,
		PlayerDash,
		PlayerTookDamage,
		BossTookDamage,
	}

	[SerializeField] private List<Sound> _sounds = new List<Sound>();

	private Dictionary<Trigger, Action<Sound>> _ActionsByTrigger = new Dictionary<Trigger, Action<Sound>> {
		{ Trigger.PlayerFootsteps, (sound) => OnDefineSoundPlayerFootsteps?.Invoke(sound) },
		{ Trigger.PlayerDash, (sound) => OnDefineSoundPlayerDash?.Invoke(sound) },
		{ Trigger.PlayerTookDamage, (sound) => OnDefineSoundPlayerTookDamage?.Invoke(sound) },
		{ Trigger.BossTookDamage, (sound) => OnDefineSoundBossTookDamage?.Invoke(sound) },
	};

	private void Start() {
		foreach (Sound sound in _sounds)
			_ActionsByTrigger[sound.trigger](sound);
	}
	public static void ChangeSound(out AudioClip audioClip, Sound sound) => audioClip = sound.audio;

	public static void PlaySoundOnce(AudioSource audioSource, AudioClip audioClip) => audioSource.PlayOneShot(audioClip);
}
