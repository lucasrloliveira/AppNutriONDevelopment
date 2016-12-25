using UnityEngine;
using System.Collections;


public class PopupUI : MonoBehaviour
{

    [SerializeField]
    float m_time = 0.3f;

    protected virtual void OnEnable()
    {
        Show();
    }

    bool _isShowing = false;
    public void Show()
    {
        if (gameObject.activeSelf)
        {
            _isShowing = true;
            transform.localScale = Vector3.zero;
            iTween.ScaleTo(gameObject, Vector3.one, m_time);
        }
        else
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }
    }

    public void Hide()
    {
        _isShowing = false;
        transform.localScale = Vector3.one;
        iTween.ScaleTo(gameObject, Vector3.zero, m_time);
    }

    protected virtual void Update()
    {
        if (!_isShowing && transform.localScale == Vector3.zero)
        {
            transform.SetAsFirstSibling();
            gameObject.SetActive(false);
        }
    }
}
