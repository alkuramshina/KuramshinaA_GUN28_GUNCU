using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class InputManager : MonoBehaviour
{
    private UnitySceneController _sceneController;
    private Controls.GameActions _controls;

    [SerializeField] private GameObject restartUI;
    [SerializeField] private Image restartFiller;
    [SerializeField] private float fillingSpeed;

    private Coroutine _restarting;

    private void Start()
    {
        _controls.Restart.performed += OnRestartPerformed;
        _controls.Restart.canceled += OnRestartCancelled;
    }

    private void OnDestroy()
    {
        _controls.Restart.performed -= OnRestartPerformed;
        _controls.Restart.canceled -= OnRestartCancelled;
    }

    private void OnRestartPerformed(InputAction.CallbackContext ctx)
    {
        restartUI.SetActive(true);
        _restarting = StartCoroutine(Restarter());
    }

    private void OnRestartCancelled(InputAction.CallbackContext ctx)
    {
        StopCoroutine(_restarting);

        restartUI.SetActive(false);
        restartFiller.fillAmount = 0f;
    }

    private IEnumerator Restarter()
    {
        while (true)
        {
            restartFiller.fillAmount += fillingSpeed * Time.deltaTime;

            if (restartFiller.fillAmount >= 1f)
            {
                _sceneController.OpenGameScene();
            }
            
            yield return null;
        }
    }

    [Inject]
    private void Init(Controls.GameActions controls)
    {
        _controls = controls;
    }
}
