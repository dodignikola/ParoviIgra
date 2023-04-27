using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private string targetMessage;

    [SerializeField] private Color highlightColor = Color.red;
    [SerializeField] Vector3 buttonPressScaleVector = new Vector3(0.2f,0.2f,1.0f);
    [SerializeField] Vector3 buttonReleaseScaleVector = new Vector3(0.3f,0.3f,1.0f);
    //private SpriteRenderer _spriteRenderer;


    private void Awake()
    {
        ///*SpriteRenderer _spriteRenderer = GetComponent<SpriteRenderer>();*/
    }

    private void OnMouseOver()
    {
        SpriteRenderer _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer != null)
            _spriteRenderer.color = highlightColor;
    }

    private void OnMouseExit()
    {
        SpriteRenderer _spriteRenderer = GetComponent<SpriteRenderer>();


        if (_spriteRenderer != null)
            _spriteRenderer.color = Color.white;
    }

    private void OnMouseDown()
    {
        transform.localScale = buttonPressScaleVector;
    }

    private void OnMouseUp()
    {
        transform.localScale = buttonReleaseScaleVector;

        if (targetObject != null)
            targetObject.SendMessage(targetMessage);
    }
}
