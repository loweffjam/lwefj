using System;
using UnityEngine;

[Serializable]
public class Sound {
	public AudioManager.Trigger trigger;
	public AudioClip audio;

	public Sound(AudioManager.Trigger trigger, AudioClip audio) {
		this.trigger = trigger;
		this.audio = audio;
	}
}
