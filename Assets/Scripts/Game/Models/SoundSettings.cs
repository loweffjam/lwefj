using System;
using UnityEngine;

[Serializable]
public class SoundSettings {
	public AudioManager.Trigger trigger;
	public AudioSource emitterObject;

	public SoundSettings(AudioManager.Trigger trigger, AudioSource emitterObject) {
		this.trigger = trigger;
		this.emitterObject = emitterObject;
	}
}