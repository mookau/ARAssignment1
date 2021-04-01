using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILookAtCamera : MonoBehaviour
{

    [SerializeField]
    private float yOffset = 0.3f;
    private float scaleOffset = 1f;

    private TMP_Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var textObject in this.gameObject.GetComponentsInChildren<TMP_Text>())
        {
            if (textObject.name == "ValueText")
            {
                healthText = textObject;
            }
        }
        scaleOffset = this.transform.root.localScale.magnitude;
        this.transform.localScale *= scaleOffset;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = this.transform.root.position + new Vector3(0f, yOffset * scaleOffset);
        this.transform.LookAt(Camera.main.transform.position);
    }

    public void UpdateHealthText(string value)
    {
        try
        {
            healthText.text = value;
        }
        catch
        {
            //Nothing to see here, move along Rick
        }
    }
}
