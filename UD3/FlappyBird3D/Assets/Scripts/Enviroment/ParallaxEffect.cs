using System;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [Serializable]
    public struct ParallaxLayer
    {
        public GameObject[] layerImages;
        public float layerSpeed;
    }

    [Header("Layer Settings")]
    [SerializeField]
    private ParallaxLayer[] parallaxLayers;

    [Header("Movement Settings")]
    [SerializeField]
    private float defaultParallaxSpeed;

    [SerializeField]
    private Camera targetCamera;

    [Header("Tiling Settings")]
    [SerializeField]
    private float tileWidth = 0f;

    [SerializeField]
    private float spacing = 0f;

    [SerializeField]
    private int bufferImageCount = 2;

    private void Start()
    {
        AlignLayerImages();
    }

    private void Update()
    {
        float parallaxSpeed = defaultParallaxSpeed;

        float cameraHalfWidth = targetCamera.orthographicSize * targetCamera.aspect;
        float cameraLeft = targetCamera.transform.position.x - cameraHalfWidth;
        float cameraRight = targetCamera.transform.position.x + cameraHalfWidth;

        foreach (ParallaxLayer layer in parallaxLayers)
        {
            if (layer.layerImages == null || layer.layerImages.Length == 0)
                continue;

            float currentTileWidth = GetTileWidth(layer.layerImages);
            float layerStep = currentTileWidth + spacing;
            float layerTotalWidth = layerStep * layer.layerImages.Length;

            float movement = parallaxSpeed * layer.layerSpeed * Time.deltaTime;
            float bufferDistance = layerStep * bufferImageCount;

            foreach (GameObject layerImage in layer.layerImages)
            {
                if (layerImage == null)
                    continue;

                Vector3 position = layerImage.transform.position;
                position.x += movement;

                if (movement > 0f &&
                    position.x - currentTileWidth * 0.5f > cameraRight + bufferDistance)
                {
                    position.x -= layerTotalWidth;
                }
                else if (movement < 0f &&
                         position.x + currentTileWidth * 0.5f < cameraLeft - bufferDistance)
                {
                    position.x += layerTotalWidth;
                }

                layerImage.transform.position = position;
            }
        }
    }

    private void AlignLayerImages()
    {
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            if (layer.layerImages == null || layer.layerImages.Length == 0)
                continue;

            float currentTileWidth = GetTileWidth(layer.layerImages);
            float layerStep = currentTileWidth + spacing;

            // Uses the first image as the anchor, then spaces every other image exactly.
            float firstX = layer.layerImages[0].transform.position.x;

            for (int i = 0; i < layer.layerImages.Length; i++)
            {
                if (layer.layerImages[i] == null)
                    continue;

                Vector3 position = layer.layerImages[i].transform.position;
                position.x = firstX + layerStep * i;
                layer.layerImages[i].transform.position = position;
            }
        }
    }

    private float GetTileWidth(GameObject[] layerImages)
    {
        if (tileWidth > 0f)
            return tileWidth;

        foreach (GameObject layerImage in layerImages)
        {
            if (layerImage == null)
                continue;

            SpriteRenderer spriteRenderer = layerImage.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
                return spriteRenderer.bounds.size.x;
        }

        return 1f;
    }
}