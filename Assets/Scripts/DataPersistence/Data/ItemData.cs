using Scriptable_Objects.Items.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace DataPersistence.Data
{
    [System.Serializable]
    public class ItemData
    {
        public string itemName;
        public int amount;
        public Sprite icon;
        // public string iconPath;
        public string itemDescription;

        public ItemData(Item item)
        {
            this.itemName = item.itemName;
            this.amount = item.amount;
            this.icon = item.icon;
            // this.icon = ConvertSpriteToBytes(item.icon);
            // this.iconPath = AssetDatabase.GetAssetPath(item.icon);
            this.itemDescription = item.itemDescription;
        }

        // Convert Sprite to byte array
        private byte[] ConvertSpriteToBytes(Sprite sprite)
        {
            if (sprite == null)
                return null;

            // Get the raw texture data
            Texture2D texture = sprite.texture;
            byte[] textureData = texture.GetRawTextureData();

            // Check if the texture format is supported for encoding
            if (texture.format != TextureFormat.DXT1 && texture.format != TextureFormat.DXT5)
            {
                // If it's not a compressed format, use EncodeToPNG
                return texture.EncodeToTGA();
            }
            
            // Handle compressed formats differently if needed
            // You might need a custom solution based on your use case
            Debug.LogWarning("Unsupported texture format for encoding: " + texture.format);
            return null;
        }

        public static Sprite ConvertBytesToSprite(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            // Create a new texture and load the image data into it
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);

            // Create a sprite from the texture
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

            return sprite;
        }
    }
}