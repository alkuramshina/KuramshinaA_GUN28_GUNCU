using System;
using UnityEngine;

namespace DefaultNamespace
{
	[RequireComponent(typeof(PositionSaver))]
	public class ReplayMover : MonoBehaviour
	{
		private PositionSaver _save;

		private int _index;
		private PositionSaver.Data _prev;
		private float _duration;

		private void Start()
		{
			////todo comment: зачем нужны эти проверки?
			/// проверка, существуют ли записи и есть ли что воспроизводить
			if (!TryGetComponent(out _save) || _save.Count() == 0)
			{
				Debug.LogError("Records incorrect value", this);
				//todo comment: Для чего выключается этот компонент?
				// чтобы не вызывался далее описанный Update,
				// который должен работать с несуществующими здесь сохранениями 
				enabled = false;
			}
		}

		private void Update()
		{
			var curr = _save.GetRecord(_index);
			//todo comment: Что проверяет это условие (с какой целью)? 
			// Если наступил конец текущей записи
			if (Time.time > curr.Time)
			{
				_prev = curr;
				_index++;
				//todo comment: Для чего нужна эта проверка?
				// Если мы дошли до конца набора записей
				if (_index >= _save.Count())
				{
					enabled = false;
					Debug.Log($"<b>{name}</b> finished", this);
				}
			}
			//todo comment: Для чего производятся эти вычисления (как в дальнейшем они применяются)?
			// по сути это скорость движения каждый кадр
			var delta = (Time.time - _prev.Time) / (curr.Time - _prev.Time);
			//todo comment: Зачем нужна эта проверка?
			// произошло деление на ноль, скорость обнулили
			if (float.IsNaN(delta)) delta = 0f;
			//todo comment: Опишите, что происходит в этой строчке так подробно, насколько это возможно
			// За этот кадр двигаем позицию текущего объекта со скоростью, записанной в сохраненных данных позиции
			// Лерп нужен для интерполяции и плавности перемещения
			transform.position = Vector3.Lerp(_prev.Position, curr.Position, delta);
		}
	}
}