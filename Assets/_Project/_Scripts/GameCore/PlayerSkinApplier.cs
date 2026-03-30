using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    public class PlayerSkinApplier : MonoBehaviour
    {
        [SerializeField] private Image _playerImage;

        [Tooltip("—прайты скинов в пор€дке индексов (0Ц5)")]
        [SerializeField] private Sprite[] _skinSprites;

        private void Start()
        {
            ApplySelectedSkin();
        }

        public void ApplySelectedSkin()
        {
            int index = SkinManager.GetSelectedSkinIndex();

            if (index >= 0 && index < _skinSprites.Length)
            {
                _playerImage.sprite = _skinSprites[index];
            }
        }
    }
}