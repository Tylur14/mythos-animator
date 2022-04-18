using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FlexMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector2 referenceResolution = new Vector2(1920,1080);
    [SerializeField] private int verticalFlexValue;   // -1, 0 ,1
    [SerializeField] private int horizontalFlexValue; // -1, 0 ,1
    [Range(0.1f,64f)][SerializeField] private float edgeRange;

    [SerializeField] private Vector2 minBounds;
    
    [Header("Debug")] 
    [SerializeField] private Vector2 currentMousePos;
    [SerializeField] private float currentHeight;
    [SerializeField] private float currentWidth;
    [SerializeField] private float requiredMouseVerticalPos;
    [SerializeField] private float requiredMouseHorizontalPos;
    [SerializeField] private float scaleFactor;

    private Vector2 _lastPosition;
    private RectTransform _transformRect;
    private bool _isResizing;
    private bool _inRange;
    
    private void Awake()
    {
        _transformRect = GetComponent<RectTransform>();
        scaleFactor = Screen.width / referenceResolution.x;
        _isResizing = false;
    }

    void Update()
    {
        var sizeDelta = _transformRect.sizeDelta;
        currentHeight = sizeDelta.y;
        currentWidth = sizeDelta.x;
        
        currentMousePos = Input.mousePosition;
        
        if (!_isResizing)
        {
            if (!_inRange) return;
            
            var anchoredPosition = _transformRect.anchoredPosition;
            
            requiredMouseVerticalPos = anchoredPosition.y * scaleFactor;
            requiredMouseHorizontalPos = anchoredPosition.x * scaleFactor;

            SetFlexValue(ref horizontalFlexValue, requiredMouseHorizontalPos,currentMousePos.x, currentWidth);
            SetFlexValue(ref verticalFlexValue, requiredMouseVerticalPos, currentMousePos.y,currentHeight);

        }

        if (Input.GetMouseButton(0))
        {
            ResizeWindow(currentMousePos - _lastPosition);
            _isResizing = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isResizing = false;
        }
        _lastPosition = currentMousePos;
    }

    void SetFlexValue(ref int flex, float pos, float testPos, float size)
    {
        if (testPos <= pos + edgeRange &&
            testPos >= pos - edgeRange)
            flex = -1;
        else if(testPos <= (pos+size * scaleFactor) + edgeRange &&
                testPos >= (pos+size * scaleFactor) - edgeRange)
            flex = 1;
        else flex = 0;
    }
    
    void ResizeWindow(Vector2 difference)
    {
        var scaleBy = difference;
        scaleBy.x *= (horizontalFlexValue) / scaleFactor;
        scaleBy.y *= (verticalFlexValue) / scaleFactor;
        var scale = _transformRect.sizeDelta;
        scale.x += scaleBy.x;
        scale.x = Mathf.Clamp(scale.x, minBounds.x, referenceResolution.x);
        scale.y += scaleBy.y;
        scale.y = Mathf.Clamp(scale.y, minBounds.y, referenceResolution.y);
        _transformRect.sizeDelta = scale;
        var pos = _transformRect.localPosition;
        if (horizontalFlexValue == -1 && scale.x > minBounds.x)
        {   
            pos.x += difference.x / scaleFactor;
        }
        if (verticalFlexValue == -1 && scale.y > minBounds.y)
        {
            pos.y += difference.y / scaleFactor;
        }

        _transformRect.localPosition = pos;

        
        //var scale = _transformRect.sizeDelta;
        //scale += difference;


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _inRange = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _inRange = false;
    }
}
