using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
	[SerializeField]
	private float _moveTime = 1f;
	[SerializeField]
	private float _delayTime = 2f;
	[SerializeField]
	private Vector3[] _positions;

	private IEnumerator Start()
    {
		if(_positions.Length < 2) yield break;
		int prev = 0, curr = 1;
		var time = 0f;
		var transform = this.transform;
		while(true)
		{
			transform.position = Vector3.Lerp(_positions[prev], _positions[curr], time / _moveTime);
			time += Time.deltaTime;
			if(time >= _moveTime)
			{
				time = 0f;
				prev = curr;
				curr = (curr + 1) % _positions.Length;
				yield return new WaitForSeconds(_delayTime);
			}

			yield return null;
		}
	}
}
