using System;
using UnityEngine;
using UnityEngine.UI;
using View.Button;

namespace View.UI.Menu
{
    public class ShopItemView : MonoBehaviour
    {
        [Header("=== Настройки скина ===")]
        [SerializeField] private int _skinIndex;
        [SerializeField] private int _price;
        [SerializeField] private bool _isDefault;

        [Header("=== Превью скина ===")]
        [SerializeField] private Image _previewImage;
        [SerializeField] private Sprite _previewWithPrice;
        [SerializeField] private Sprite _previewClean;

        [Header("=== Кнопка действия ===")]
        [SerializeField] private CustomButton _actionButton;
        [SerializeField] private Image _buttonImage;
        [SerializeField] private Sprite _buyButtonSprite;
        [SerializeField] private Sprite _selectButtonSprite;
        [SerializeField] private Sprite _selectedButtonSprite;

        public event Action OnStateChanged;

        public int SkinIndex => _skinIndex;


        private void OnEnable()
        {
            _actionButton.AddListener(HandleClick);
            Refresh();
        }

        private void OnDisable()
        {
            _actionButton.RemoveListener(HandleClick);
        }


        public void Refresh()
        {
            bool owned = _isDefault || GameCore.SkinManager.IsSkinOwned(_skinIndex);
            bool selected = GameCore.SkinManager.GetSelectedSkinIndex() == _skinIndex;

            if (_previewImage != null)
            {
                _previewImage.sprite = owned ? _previewClean : _previewWithPrice;
            }

            if (!owned)
            {
                // Ещё не куплен
                _buttonImage.sprite = _buyButtonSprite;
                bool canAfford = GameCore.SkinManager.GetCurrentParts() >= _price;
                //_actionButton.Interactable = canAfford;
            }
            else if (selected)
            {
                // Куплен и выбран
                _buttonImage.sprite = _selectedButtonSprite;
                //_actionButton.Interactable = true;
            }
            else
            {
                // Куплен, но НЕ выбран — можно выбрать
                _buttonImage.sprite = _selectButtonSprite;
                //_actionButton.Interactable = true;
            }
        }


        private void HandleClick()
        {
            bool owned = _isDefault || GameCore.SkinManager.IsSkinOwned(_skinIndex);

            if (!owned)
            {
                TryPurchase();
            }
            else
            {
                TrySelect();
            }
        }

        private void TryPurchase()
        {
            if (!GameCore.SkinManager.TrySpendParts(_price)) return;

            GameCore.SkinManager.PurchaseSkin(_skinIndex);
            GameCore.SkinManager.SelectSkin(_skinIndex);   // авто-выбор после покупки

            OnStateChanged?.Invoke();
        }

        private void TrySelect()
        {
            // Если уже выбран — ничего
            if (GameCore.SkinManager.GetSelectedSkinIndex() == _skinIndex) return;

            GameCore.SkinManager.SelectSkin(_skinIndex);
            OnStateChanged?.Invoke();
        }
    }
}