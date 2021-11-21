using UnityEngine;

public class Tweener : MonoBehaviour
{
    public UIElement[] uIElements;
    public bool isStartingFirst;

    private void Start()
    {
        if (isStartingFirst)
        {
            UIFront();
        }
    }

    public void UIFront()
    {
        foreach (UIElement uIElement in uIElements)
        {
            LeanTween.moveLocal(uIElement.objectToAnimate, uIElement.inPosition, uIElement.secondsToMove).setEase(uIElement.easeType).setIgnoreTimeScale(false);
        }
    }

    public void UIBack()
    {
        foreach (UIElement uIElement in uIElements)
        {
            LeanTween.moveLocal(uIElement.objectToAnimate, uIElement.outPosition, uIElement.secondsToMove).setEase(uIElement.easeType).setIgnoreTimeScale(false);
        }
    }
}