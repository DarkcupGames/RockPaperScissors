using DG.Tweening;
using TMPro;
using UnityEngine;

public class ResultPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultTxt;
    private RectTransform resultPopupRect;
    private void Awake()
    {
        resultPopupRect = GetComponent<RectTransform>();
    }
    public void Show(string result)
    {
        resultPopupRect.DOSizeDelta(new Vector2(resultPopupRect.sizeDelta.x, 200), 1f).OnComplete(() =>
        {
            resultTxt.text = result;
            resultTxt.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        });
    }

    public void Hide()
    {
        resultPopupRect.DOSizeDelta(new Vector2(resultPopupRect.sizeDelta.x, 0), 1f).OnComplete(() =>
        {
        });
        resultTxt.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.InBack);
    }
}