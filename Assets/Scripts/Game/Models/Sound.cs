using System;
using UnityEngine;

[Serializable]
public class Sound {
	public AudioManager.Trigger trigger;
	public AudioClip audio;
	public GameObject emitterObject;

	public Sound(AudioManager.Trigger trigger, AudioClip audio, GameObject emitterObject) {
		this.trigger = trigger;
		this.audio = audio;
		this.emitterObject = emitterObject;
	}
}
