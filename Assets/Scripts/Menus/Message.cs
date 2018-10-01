using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour {
	private Animator animator;
	
	private void Awake() {
		animator = GetComponent<Animator>();
		gameObject.SetActive(false);
	}

	public void open() {
		gameObject.SetActive(true);
	}

	public void close() {
		animator.SetTrigger("popOut");
	}
}
