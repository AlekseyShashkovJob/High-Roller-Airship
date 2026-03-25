using UnityEngine;
using View.Button;

namespace View.UI.Menu
{
    public class PrivacyScreen : UIScreen
    {
        [SerializeField] private CustomButton _back;

        private void OnEnable()
        {
            _back.AddListener(BackToMenu);
        }

        private void OnDisable()
        {
            _back.RemoveListener(BackToMenu);
        }

        public override void SetupScreen(UIScreen previousScreen)
        {

        }

        private void BackToMenu()
        {
            CloseScreen();
        }
    }
}