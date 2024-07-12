using System;
using System.Collections;

using UnityEngine;

public class CameraRotator : MonoBehaviour
{
	[SerializeField]
	private Transform _target;
	[SerializeField, Min(0.1f)]
	private float _speed = 1f;

	private void Start()
	{
		StartCoroutine(Rotator());
	}

	private IEnumerator Rotator()
	{
		var transform = this.transform;
		while(true)
		{
			var rotation = Quaternion.LookRotation(_target.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _speed);
			yield return new WaitForEndOfFrame();
		}
	}
}