using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipWindow : MonoBehaviour
{
    public GameObject objWindowContent;
    public TextMeshProUGUI txtText;

    public Boolean checkBiomeCompletenessAfterClosing;

    public void Close()
    {
        if (checkBiomeCompletenessAfterClosing)
            LeanTween.delayedCall(0.2f, BoardController.Instance.CheckIfBoardCompleted);

        Destroy(gameObject);
    }

    private void Awake()
    {
        objWindowContent.transform.localScale = Vector3.zero;
    }

    public void Show(string text)
    {
        txtText.text = text;
        LeanTween.scale(objWindowContent, Vector3.one, 0.5f)
            .setEaseOutSine();

    }    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
