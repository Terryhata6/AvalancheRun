using UnityEngine;
using UnityEngine.UI;

public class InGameView : BaseMenuView
{
    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    [Header("Elements")]
    [SerializeField] private Button _pauseButton;
    [SerializeField] private TextMesh _moneyText;

    private UIController _controller;


    private void Start()
    {
        FindMyController();
        _pauseButton.onClick.AddListener(UIEvents.Current.ButtonPauseGame);
        GameEvents.current.OnAddMoney += UpdateMoneyText;
    }

    public override void Hide()
    {
        if (!IsShow) return;
        _panel.gameObject.SetActive(false);
        IsShow = false;
    }

    public override void Show()
    {
        if (IsShow) return;
        _panel.gameObject.SetActive(true);
        IsShow = true;
    }

    public void FindMyController()
    {
        if (_controller == null)
        {
            _controller = FindObjectOfType<MainController>().GetController<UIController>();
        }
        _controller.AddView(this);
    }

    private void UpdateMoneyText(int value)
    {

    }
}