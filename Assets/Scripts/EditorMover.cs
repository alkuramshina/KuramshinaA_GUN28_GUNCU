using UnityEngine;

namespace DefaultNamespace
{
	
	[RequireComponent(typeof(PositionSaver))]
	public class EditorMover : MonoBehaviour
	{
		private PositionSaver _save;
		private float _currentDelay;
		
		//todo comment: Что произойдёт, если _delay > _duration?
		// enabled станет false раньше, чем произойдет хотя бы одна запись
		[SerializeField, Range(0.2f, 1.0f)] private float _delay = 0.5f;
		[SerializeField, Min(0.2f)] private float _duration = 5f;

		private void Start()
		{
			//todo comment: Почему этот поиск производится здесь, а не в начале метода Update?
			// Здесь он произойдет один раз после загрузки компонента, в Update - будет происходить каждый кадр
			_save = GetComponent<PositionSaver>();
			_save.Clear();
			
			if (_duration <= _delay)
			{
				_duration = _delay * 5;
			}
		}

		private void Update()
		{
			_duration -= Time.deltaTime;
			if (_duration <= 0f)
			{
				enabled = false;
				Debug.Log($"<b>{name}</b> finished", this);
				return;
			}
			
			//todo comment: Почему не написать (_delay -= Time.deltaTime;) по аналогии с полем _duration?
			// _delay будет перетираться и когда станет == 0f запись будет каждый кадр
			_currentDelay -= Time.deltaTime;
			if (_currentDelay <= 0f)
			{
				_currentDelay = _delay;
				_save.AddRecord(new PositionSaver.Data
				{
					Position = transform.position,
					//todo comment: Для чего сохраняется значение игрового времени?
					// чтобы далее воспроизвести с учетом полученной скорости движения
					Time = Time.time,
				});
			}
		}
	}
}