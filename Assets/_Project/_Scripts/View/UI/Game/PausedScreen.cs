using System;
using UnityEngine;
using View.Button;

namespace View.UI.Game
{
    public class PausedScreen : UIScreen
    {
        [SerializeField] private UIScreen _optionsScreen;

        [SerializeField] private CustomButton _continue;
        [SerializeField] private CustomButton _restart;
        [SerializeField] private CustomButton _settings;
        [SerializeField] private CustomButton _menu;

        private void OnEnable()
        {
            _continue.AddListener(ContinueGame);
            _restart.AddListener(Restart);
            _settings.AddListener(OpenOptions);
            _menu.AddListener(BackToMenu);
        }

        private void OnDisable()
        {
            _continue.RemoveListener(ContinueGame);
            _restart.RemoveListener(Restart);
            _settings.RemoveListener(OpenOptions);
            _menu.RemoveListener(BackToMenu);
        }

        public override void StartScreen()
        {
            base.StartScreen();

            Time.timeScale = 0.0f;
        }

        public override void SetupScreen(UIScreen previousScreen)
        {
            throw new NotImplementedException();
        }

        private void ContinueGame()
        {
            Time.timeScale = 1.0f;
            CloseScreen();
        }

        private void Restart()
        {
            // Time.timeScale = 1.0f;
            GameCore.GameManager.Instance.RestartGame();
            CloseScreen();
        }

        private void OpenOptions()
        {
            _optionsScreen.StartScreen();
        }

        private void BackToMenu()
        {
            //Time.timeScale = 1.0f;
            GameCore.GameManager.Instance.BackToMenu();
            CloseScreen();
        }
    }
}