using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachUIElements : MonoBehaviour
{
    [SerializeField]
    private GameObject UIPrefab;

    private UILookAtCamera cameraScript;

    // Start is called before the first frame update
    void Start()
    {
        if (UIPrefab != null)
        {
            var instantiatedCanvas = Instantiate(UIPrefab);
            instantiatedCanvas.transform.SetParent(this.transform.root);
            cameraScript = instantiatedCanvas.GetComponent<UILookAtCamera>();
        }
    }

    public void UpdateText(string text)
    {
        cameraScript.UpdateHealthText(text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
