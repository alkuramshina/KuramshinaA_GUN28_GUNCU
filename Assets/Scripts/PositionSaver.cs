using System;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace.Utils;
using UnityEngine;

namespace DefaultNamespace
{
	public class PositionSaver : MonoBehaviour
	{
		[Serializable]
		public struct Data
		{
			public Vector3 Position;
			public float Time;
		}

		[SerializeField, ReadOnly]
		[Tooltip("Для заполнения этого поля нужно воспользоваться контекстным меню в инспекторе и командой “Create File”")]
		private TextAsset _json;

		[SerializeField, HideInInspector]
		private List<Data> Records;

		public void Clear() => Records.Clear();
		public int Count() => Records.Count;

		public void AddRecord(Data record) => Records.Add(record);
		public Data GetRecord(int index) => Records[index];

		private void Awake()
		{
			//todo comment: Что будет, если в теле этого условия не сделать выход из метода?
			// NRE
			if (_json == null)
			{
				gameObject.SetActive(false);
				Debug.LogError("Please, create TextAsset and add in field _json");
				return;
			}
			
			JsonUtility.FromJsonOverwrite(_json.text, this);
			
			//todo comment: Для чего нужна эта проверка (что она позволяет избежать)?
			// NRE :) Проверка нужна, чтобы не перетирать данные, подтянутые из сохраненных в json, полагаю 
			if (Records == null)
				Records = new List<Data>(10);
		}

		private void OnDrawGizmos()
		{
			//todo comment: Зачем нужны эти проверки (что они позволляют избежать)?
			// Если записей о движении нет, то показывать нечего и обращаться к данным пустого объекта не нужно (NRE :D)
			if (Records == null || Records.Count == 0) return;
			var data = Records;
			var prev = data[0].Position;
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(prev, 0.3f);
			//todo comment: Почему итерация начинается не с нулевого элемента?
			// Первая позиция определена выше, гизмосы будут рисоваться от prev к curr
			for (int i = 1; i < data.Count; i++)
			{
				var curr = data[i].Position;
				Gizmos.DrawWireSphere(curr, 0.3f);
				Gizmos.DrawLine(prev, curr);
				prev = curr;
			}
		}
		
#if UNITY_EDITOR
		private readonly string _filePath = Path.Combine(Application.dataPath, "Path.txt");
		
		[ContextMenu("Create File")]
		private void CreateFile()
		{
			//todo comment: Что происходит в этой строке?
			// Создается файл по указанному пути, данные открываются для чтения и записи в потоке stream
			var stream = File.Create(_filePath);
			//todo comment: Подумайте для чего нужна эта строка? (а потом проверьте догадку, закомментировав) 
			// освобождение ресурсов (using)
			stream.Dispose();
			UnityEditor.AssetDatabase.Refresh();
			//В Unity можно искать объекты по их типу, для этого используется префикс "t:"
			//После нахождения, Юнити возвращает массив гуидов (которые в мета-файлах задаются, например)
			var guids = UnityEditor.AssetDatabase.FindAssets("t:TextAsset");
			foreach (var guid in guids)
			{
				//Этой командой можно получить путь к ассету через его гуид
				var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
				//Этой командой можно загрузить сам ассет
				var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(path);
				//todo comment: Для чего нужны эти проверки?
				// Проверка, был ли найден нужный ассет и есть ли с чем работать
				if(asset != null && asset.name == "Path")
				{
					_json = asset;
					UnityEditor.EditorUtility.SetDirty(this);
					UnityEditor.AssetDatabase.SaveAssets();
					UnityEditor.AssetDatabase.Refresh();
					//todo comment: Почему мы здесь выходим, а не продолжаем итерироваться?
					// потому что нужный ассет был найден и перебирать гуиды дальше не имеет смысла
					return;
				}
			}
		}

		private void OnDestroy()
		{
			File.WriteAllText(_filePath, JsonUtility.ToJson(this));
		}
#endif
	}
}