using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Use key Q, W, E, R to generate logs examples");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("This is a LOG generated with keydown at runtime.");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.LogWarning("This is a WARNING generated with keydown at runtime.");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.LogError("This is an ERROR generated with keydown at runtime.");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("this is a very long log: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam tincidunt at ex a lacinia. Sed ut tincidunt sem. Curabitur dignissim, lectus vel tincidunt consequat, purus eros tristique mi, eget iaculis nibh magna non quam. Nunc auctor mi in urna convallis, a hendrerit sem aliquet. Suspendisse ut nisi hendrerit, lacinia augue vel, posuere orci. Duis varius bibendum aliquam. Nulla facilisi. Nam nec maximus odio.");
        }

    }


}
