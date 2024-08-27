using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Editor
{
    public class EditorCheatWindow : EditorWindow
    {
        private EditorControls _controls;
        
        [MenuItem("Window/Cheats")]
        public static void ShowWindow()
        {
            GetWindow(typeof(EditorCheatWindow));
        }

        private void OnGUI()
        {
            GUILayout.Label("Next Turn: Key [1]");
            GUILayout.Label("Kill: Key [2]");
        }
        
		private void OnEnable() => EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		private void OnDisable() => EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

		private void OnPlayModeStateChanged(PlayModeStateChange obj)
		{
			switch (obj)
			{
				//Вход в плеймод (происходит после выхода из EditMode)
				case PlayModeStateChange.EnteredPlayMode:
					//На время игры создаем контролс и подписываемся на его экшены
					_controls = new EditorControls();
					_controls.Cheats.Enable();
					_controls.Cheats.NextTurn.performed += OnNextTurnPerformed;
					_controls.Cheats.Kill.performed += OnKillPerformed;
					break;
				//Выход из плеймода (происходит перед входом в EditMode)
				case PlayModeStateChange.ExitingPlayMode:
					//Для простоты, чтобы не париться с проверками - при выходе очищаем память
					_controls.Disable();
					_controls.Cheats.NextTurn.performed -= OnNextTurnPerformed;
					_controls.Cheats.Kill.performed -= OnKillPerformed;
					_controls.Dispose();
					_controls = null;
					break;
				//Вход в режим редактора
				case PlayModeStateChange.EnteredEditMode:
				//Выход из режима редактора
				case PlayModeStateChange.ExitingEditMode:
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
			}
		}
		
		private void OnNextTurnPerformed(InputAction.CallbackContext obj)
		{
		}

		private void OnKillPerformed(InputAction.CallbackContext obj)
		{
		}
    }
}