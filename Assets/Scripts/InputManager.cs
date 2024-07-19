using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    private SceneController _sceneController;
    private Controls.GameActions _controls;
    
    [SerializeField] private GameObject restartUI;
    [SerializeField] private Image restartFiller;
    [SerializeField] private float fillingSpeed;

    private Coroutine _restarting;
    
    private void Update()
    {
//        throw new NotImplementedException();
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
        restartFiller.fillAmount += fillingSpeed * Time.deltaTime;
        yield return null;
    }
}
