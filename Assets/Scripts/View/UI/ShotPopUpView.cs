using TMPro;
using UnityEngine;

public class ShotPopUpView : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private Color _color;
    private float _timeSeconds = 0.2f;
    private float _scaleDelta = 0.01f;
    private float _scaleGoal = 1f;


    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>(true);
        SwitchText(false);
    }

    private void Start()
    {
        GameEvents.Current.OnPlayerGetHit += ActivatePopUp;
        GameEvents.Current.OnPlayerHiterName += SetText;

        InputEvents.current.OnTouchBeganEvent += OffText;
    }

    private void ActivatePopUp()
    {
        _color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f);
        _text.color = _color;

        float textZAngle = Random.Range(-45f, 45f);
        int ticks = (int)(_scaleGoal / _scaleDelta);
        float timeDelta = _timeSeconds / ticks;

        SwitchText(true);

        _text.transform.Rotate(new Vector3(0, 0, textZAngle));
        _text.transform.localScale = new Vector3(0, 0, 1);
        for (int i = 0; i <= ticks; i++)
        {
            Invoke("Scale", timeDelta * i);
        }
    }

    private void SwitchText(bool isActive)
    {
        _text.gameObject.SetActive(isActive);
    }

    private void OffText(Vector2 vector)
    {
        _text.gameObject.SetActive(false);
    }

    private void Scale()
    {
        _text.transform.localScale += new Vector3(_scaleDelta, _scaleDelta, 0);
    }

    private void SetText(string text)
    {
        _text.text = FallingObjectsNameManager.GetShotString(text);
    }
}