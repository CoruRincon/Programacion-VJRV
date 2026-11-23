using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadingScene : MonoBehaviour {
    public Image LoadingBarFill; // Referencia a la imagen de la interfaz con el relleno
    public float minimumLoadTime = 3f; // Tiempo mínimo en segundos
    private float elapsedTime = 0f; // Tiempo transcurrido
                                    // Este método se ejecuta una vez al principio
    private void Start() {
        StartCoroutine(LoadSceneAsync("Game")); // Llamamos a la corrutina una vez
    }
    // Creamos la corrutina
    IEnumerator LoadSceneAsync(string sceneName) {
        // Creamos una Operación Asíncrona (proceso que se puede ejecutar
        // en segundo plano y de forma independientemente )
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // Con esto la escena no se activa automáticamente
                                                // No mostramos la escena cargando en segundo plano hasta que no se complete y haya pasado
                                                // el tiempo mínimo establecido (minimumLoadTime)
        while (operation.progress < 0.9f || elapsedTime < minimumLoadTime) {
            // Mientras se carga actualizamos la barra del relleno. Se divide entre 0.9f
            // porque el progreso pasa de 90 a 100 de golpe y no veríamos cómo alcanza el 100%
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            // Se usa Mathf.Clamp01 para pasar de 0-100 a un valor entre 0 y 1
            LoadingBarFill.fillAmount = progressValue;
            elapsedTime += Time.deltaTime; // Incrementamos el tiempo transcurrido
                                           // Mientras la operación no haya terminado, esperamos al siguiente frame
            yield return null;

        }
        // Permite la activación de la escena después de cumplir las condiciones
        operation.allowSceneActivation = true;
    }
}