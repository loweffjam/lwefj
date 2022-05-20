using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(AudioManager))]
public class AudioManagerInspector : Editor {
	private int currentTriggerIndex;
	private AudioSource currentEmitter;

	public override void OnInspectorGUI() {
		serializedObject.Update();
		DrawDefaultInspector();

		if (GUI.changed) {
			SerializedProperty sounds = serializedObject.FindProperty("sounds");
			SerializedProperty currentEditSound = serializedObject.FindProperty("currentEdittingSound");
			SerializedProperty lastEditSound = serializedObject.FindProperty("lastEdittingSound");
			if (currentEditSound.enumValueIndex != lastEditSound.enumValueIndex) {
				for (int i = 0; i < sounds.arraySize; i++) {
					SerializedProperty current = sounds.GetArrayElementAtIndex(i);
					foreach (SerializedProperty childProperty in current) {
						if (childProperty.name == "trigger")
							currentTriggerIndex = childProperty.enumValueIndex;
						if (childProperty.name == "emitterObject" && currentTriggerIndex == currentEditSound.enumValueIndex) {
							GameObject emitterObj = (GameObject)childProperty.objectReferenceValue;
							currentEmitter = emitterObj.GetComponent<AudioSource>();
						}
					}

					if (currentTriggerIndex == currentEditSound.enumValueIndex && currentEmitter != null) {
						AudioManager audioManager = target as AudioManager;
						GameManager.CopyAudioSource(currentEmitter, audioManager.gameObject, false);
					}
				}
				lastEditSound.enumValueIndex = currentEditSound.enumValueIndex;
			}
			serializedObject.ApplyModifiedProperties();
		}
	}
}
