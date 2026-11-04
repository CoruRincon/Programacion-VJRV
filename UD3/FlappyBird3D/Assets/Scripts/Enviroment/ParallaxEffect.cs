using System;
using UnityEngine;
public class ParallaxEffect : MonoBehaviour {

    [Serializable]
    public struct ParallaxLayer {
        public GameObject[] layerImages;
        public float layerSpeed;
    }

    [Header("Layer Settings")]
    [SerializeField]
    private ParallaxLayer[] parallaxLayers;

    [Header("Movement Settings")]
    [SerializeField]
    private float defaultParallaxSpeed;

    private Camera cam;

    private void Start() {
        cam = Camera.main;
    }

    private void Update() {
        float parallaxSpeed = defaultParallaxSpeed;

        foreach (var layer in parallaxLayers) {
            int loop = 0;
            foreach (var layerImage in layer.layerImages) {
                Vector3 currentPos = layerImage.transform.position;

                Vector3 newPos = new Vector3(currentPos.x + (parallaxSpeed * layer.layerSpeed) * Time.deltaTime, currentPos.y, currentPos.z);

                SpriteRenderer renderer = layerImage.GetComponent<SpriteRenderer>();
                float imageWidth = renderer.bounds.size.x;

                if (layerImage.transform.position.x - (imageWidth + 0.1f) >= imageWidth) {
                    newPos = new Vector3(-imageWidth, currentPos.y, currentPos.z);
                } else if(layerImage.transform.position.x + (imageWidth - 0.1f) <= -imageWidth) {
                    newPos = new Vector3(imageWidth, currentPos.y, currentPos.z);
                }

                layerImage.transform.position = newPos;

                loop++;
            }
        }
    }
}