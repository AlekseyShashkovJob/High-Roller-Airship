using UnityEngine;

namespace GameCore
{
    public static class SkinManager
    {
        private const string OWNED_PREFIX = "skin_owned_";
        private const string SELECTED_KEY = "skin_selected";

        public static bool IsSkinOwned(int skinIndex)
        {
            if (skinIndex == 0) return true; // скин по умолчанию
            return PlayerPrefs.GetInt(OWNED_PREFIX + skinIndex, 0) == 1;
        }

        public static void PurchaseSkin(int skinIndex)
        {
            PlayerPrefs.SetInt(OWNED_PREFIX + skinIndex, 1);
            PlayerPrefs.Save();
        }

        public static int GetSelectedSkinIndex()
        {
            return PlayerPrefs.GetInt(SELECTED_KEY, 0);
        }

        public static void SelectSkin(int skinIndex)
        {
            PlayerPrefs.SetInt(SELECTED_KEY, skinIndex);
            PlayerPrefs.Save();
        }

        public static int GetCurrentParts()
        {
            return PlayerPrefs.GetInt(GameConstants.TOTAL_PARTS_KEY, 0);
        }

        public static bool TrySpendParts(int amount)
        {
            int current = GetCurrentParts();
            if (current < amount) return false;

            PlayerPrefs.SetInt(GameConstants.TOTAL_PARTS_KEY, current - amount);
            PlayerPrefs.Save();
            return true;
        }
    }
}