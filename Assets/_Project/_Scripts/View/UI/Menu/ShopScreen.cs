using UnityEngine;
using TMPro;
using View.Button;

namespace View.UI.Menu
{
    public class ShopScreen : UIScreen
    {
        [Header("=== Страницы ===")]
        [SerializeField] private GameObject _page1;
        [SerializeField] private GameObject _page2;

        [Header("=== Навигация ===")]
        [SerializeField] private CustomButton _nextButton;
        [SerializeField] private CustomButton _prevButton;
        [SerializeField] private CustomButton _returnButton;

        [Header("=== Очки ===")]
        [SerializeField] private TMP_Text _partsText;

        [Header("=== Все слоты магазина (6 штук) ===")]
        [SerializeField] private ShopItemView[] _shopItems;

        private UIScreen _previousScreen;

        private void OnEnable()
        {
            _nextButton.AddListener(OpenPage2);
            _prevButton.AddListener(OpenPage1);
            _returnButton.AddListener(ReturnToMainMenu);

            foreach (var item in _shopItems)
                item.OnStateChanged += OnAnyItemChanged;

            OpenPage1();
            RefreshAll();
        }

        private void OnDisable()
        {
            _nextButton.RemoveListener(OpenPage2);
            _prevButton.RemoveListener(OpenPage1);
            _returnButton.RemoveListener(ReturnToMainMenu);

            foreach (var item in _shopItems)
                item.OnStateChanged -= OnAnyItemChanged;
        }

        public override void SetupScreen(UIScreen previousScreen)
        {
            if (_previousScreen == null)
                _previousScreen = previousScreen;
        }

        private void OnAnyItemChanged()
        {
            RefreshAll();
        }

        private void RefreshAll()
        {
            UpdatePartsText();

            // Обновляем ВСЕ слоты, чтобы сбросить «Selected» у ранее выбранного
            foreach (var item in _shopItems)
                item.Refresh();
        }

        private void UpdatePartsText()
        {
            int parts = PlayerPrefs.GetInt(GameCore.GameConstants.TOTAL_PARTS_KEY, 0);
            _partsText.text = $"{parts}";
        }

        private void OpenPage1()
        {
            _page1.SetActive(true);
            _page2.SetActive(false);
        }

        private void OpenPage2()
        {
            _page1.SetActive(false);
            _page2.SetActive(true);
        }

        private void ReturnToMainMenu()
        {
            CloseScreen();
            _previousScreen.StartScreen();
        }
    }
}