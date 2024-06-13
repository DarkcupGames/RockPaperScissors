using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winTxt;
    [SerializeField] private RectTransform resultPopupRect;
    public void Show(string result)
    {
        gameObject.SetActive(true);
        resultPopupRect.transform.localScale = Vector3.zero;
        winTxt.transform.localScale = Vector3.zero;
        resultPopupRect.DOScale( Vector2.one, 1f).OnComplete(() =>
        {
            winTxt.text = result;
            winTxt.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        });
    }

    public void Hide()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
