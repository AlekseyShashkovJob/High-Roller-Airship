using UnityEngine;
using System;
using View.Button;

namespace View.UI.Menu
{
    public class MainMenuScreen : UIScreen
    {
        [SerializeField] private Misc.SceneManagment.SceneLoader _sceneLoader;

        [SerializeField] private UIScreen _settingsScreen;
        [SerializeField] private UIScreen _shopScreen;
        [SerializeField] private UIScreen _privacyScreen;

        [SerializeField] private CustomButton _startGame;
        [SerializeField] private CustomButton _shop;
        [SerializeField] private CustomButton _privacy;
        [SerializeField] private CustomButton _settings;

        private void OnEnable()
        {
            _shopScreen.SetupScreen(this);

            _startGame.AddListener(OpenGame);
            _privacy.AddListener(OpenPrivacy);
            _settings.AddListener(OpenSettings);
            _shop.AddListener(OpenShop);
        }

        private void OnDisable()
        {
            _startGame.RemoveListener(OpenGame);
            _privacy.RemoveListener(OpenPrivacy);
            _settings.RemoveListener(OpenSettings);
            _shop.RemoveListener(OpenShop);
        }

        public override void SetupScreen(UIScreen previousScreen)
        {
            throw new NotImplementedException();
        }

        private void OpenGame()
        {
            _sceneLoader.ChangeScene(Misc.Data.SceneConstants.GAME_SCENE);
            CloseScreen();
        }

        private void OpenPrivacy()
        {
            _privacyScreen.StartScreen();
        }

        private void OpenSettings()
        {
            _settingsScreen.StartScreen();
        }

        private void OpenShop()
        {
            _shopScreen.StartScreen();
            CloseScreen();
        }
    }
}