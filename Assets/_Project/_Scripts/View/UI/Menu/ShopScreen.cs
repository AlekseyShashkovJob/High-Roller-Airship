using UnityEngine;
using TMPro;
using View.Button;

namespace View.UI.Menu
{
    public class ShopScreen : UIScreen
    {
        [SerializeField] private GameObject _page1;
        [SerializeField] private GameObject _page2;

        [SerializeField] private CustomButton _nextButton;
        [SerializeField] private CustomButton _prevButton;
        [SerializeField] private CustomButton _returnButton;

        [SerializeField] private TMP_Text _eggsText;

        private UIScreen _previousScreen;

        private void OnEnable()
        {
            UpdateCoinsText();
            OpenPage1();

            _nextButton.AddListener(OpenPage2);
            _prevButton.AddListener(OpenPage1);
            _returnButton.AddListener(ReturnToMainMenu);
        }

        private void OnDisable()
        {
            _nextButton.RemoveListener(OpenPage2);
            _prevButton.RemoveListener(OpenPage1);
            _returnButton.RemoveListener(ReturnToMainMenu);
        }

        public override void SetupScreen(UIScreen previousScreen)
        {
            if (_previousScreen == null)
            {
                _previousScreen = previousScreen;
            }
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

        private void UpdateCoinsText()
        {
            int currentEggs = PlayerPrefs.GetInt(GameCore.GameConstants.TOTAL_DETAILS_KEY, 0);
            _eggsText.text = $"{currentEggs}";
        }
    }
}