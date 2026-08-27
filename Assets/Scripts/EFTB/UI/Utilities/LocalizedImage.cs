using JumboJumps.EFTB.Constant.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace JumboJumps.EFTB.UI.Utilities
{
    [RequireComponent(typeof(Image))]
    public class LocalizedImage : MonoBehaviour
    {
        [Header("Target Components")]
        [SerializeField] private Image targetImage;

        [Header("Default Localization Settings")]
        [SerializeField] private LocalizedAsset<Sprite> localizedSprite = new LocalizedAsset<Sprite>();

        private void Awake()
        {
            if (targetImage == null)
            {
                targetImage = GetComponent<Image>();
            }
        }

        private void OnEnable()
        {
            if (localizedSprite != null)
            {
                localizedSprite.AssetChanged += OnAssetChanged;
                localizedSprite.LoadAssetAsync();
            }
        }

        private void OnDisable()
        {
            if (localizedSprite != null)
            {
                localizedSprite.AssetChanged -= OnAssetChanged;
            }
        }

        public void SetSprite(Sprite sprite)
        {
            if (localizedSprite != null)
            {
                localizedSprite.AssetChanged -= OnAssetChanged;
            }
            if (targetImage != null)
            {
                targetImage.sprite = sprite;
            }
        }

        public void SetLocalizedKey(string entryKey, string table = ConstLocalization.ASSET_TABLE)
        {
            if (localizedSprite != null)
            {
                localizedSprite.AssetChanged -= OnAssetChanged;
            }

            localizedSprite = new LocalizedAsset<Sprite>
            {
                TableReference = table,
                TableEntryReference = entryKey
            };

            localizedSprite.AssetChanged += OnAssetChanged;
            localizedSprite.LoadAssetAsync();
        }

        private void OnAssetChanged(Sprite value)
        {
            if (targetImage != null && value != null)
            {
                targetImage.sprite = value;
            }
        }
    }
}
