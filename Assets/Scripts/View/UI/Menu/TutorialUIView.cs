using UnityEngine;

public class TutorialUIView : BaseMenuView
{
    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    private UIController _controller;


    private void Awake()
    {
        FindMyController();
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
}
