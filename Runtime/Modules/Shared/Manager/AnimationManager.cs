using UnityEngine;

/// <summary>
/// <description>
/// This class is used to create animations for UI elements
/// </description><br/>
/// @author:  Anthony Shinomiya M.
/// @date:  2022-01-22
/// </summary>
public static class AnimationManager
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public static LTDescr AnimateIn(CanvasGroup canvasGroup,
        float duration = 1f,
        Direction direction = Direction.Down,
        float distance = 100f,
        float delay = 0f,
        LeanTweenType easeType = LeanTweenType.easeInOutSine,
        bool activeBlockRaycastsOnStart = false
        )
    {
        if (canvasGroup == null)
        {
            Debug.LogWarning("CanvasGroup is null");
            return null;
        }

        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        // canvasGroup.interactable = true;

        Vector3 initialPosition = Vector3.zero;
        Vector3 finalPosition = initialPosition;

        switch (direction)
        {
            case Direction.Up:
                finalPosition.y -= distance;
                break;
            case Direction.Down:
                finalPosition.y += distance;
                break;
            case Direction.Left:
                finalPosition.x -= distance;
                break;
            case Direction.Right:
                finalPosition.x += distance;
                break;
        }

        canvasGroup.transform.localPosition = finalPosition;
        if (activeBlockRaycastsOnStart)
        {
            canvasGroup.blocksRaycasts = true;
        }
        LeanTween.cancel(canvasGroup.gameObject);
        LeanTween.alphaCanvas(canvasGroup, 1, duration).setEase(easeType).setOnComplete(() =>
        {
            canvasGroup.blocksRaycasts = true;
            // canvasGroup.interactable = true;
        }).setDelay(delay);
        return LeanTween.moveLocal(canvasGroup.gameObject, Vector3.zero, duration).setEase(easeType).setDelay(delay);
    }

    public static LTDescr AnimateOut(CanvasGroup canvasGroup,
        float duration = 1f,
        Direction direction = Direction.Down,
        float distance = 100f,
        float delay = 0f,
        LeanTweenType easeType = LeanTweenType.easeInOutSine
        )
    {
        if (canvasGroup == null)
        {
            Debug.LogWarning("CanvasGroup is null");
            return null;
        }

        canvasGroup.blocksRaycasts = false;

        Vector3 initialPosition = canvasGroup.transform.localPosition;
        Vector3 finalPosition = initialPosition;

        switch (direction)
        {
            case Direction.Up:
                finalPosition.y += distance;
                break;
            case Direction.Down:
                finalPosition.y -= distance;
                break;
            case Direction.Left:
                finalPosition.x -= distance;
                break;
            case Direction.Right:
                finalPosition.x += distance;
                break;
        }

        LeanTween.cancel(canvasGroup.gameObject);
        LeanTween.alphaCanvas(canvasGroup, 0, duration).setEase(easeType).setOnComplete(() =>
        {
            canvasGroup.blocksRaycasts = false;
            // canvasGroup.interactable = false;
        }).setDelay(delay);
        return LeanTween.moveLocal(canvasGroup.gameObject, finalPosition, duration).setEase(easeType).setDelay(delay);
    }
}