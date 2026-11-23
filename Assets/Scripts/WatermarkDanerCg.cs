using UnityEngine;

/// Marca de agua "DanerCg" en la esquina inferior derecha.
/// Se crea sola al iniciar el juego: no hay que ponerla en la escena.
public class WatermarkDanerCg : MonoBehaviour
{
    const string Texto = "DanerCg";
    static WatermarkDanerCg instance;
    GUIStyle style;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Crear()
    {
        if (instance != null) return;
        var go = new GameObject("Watermark_DanerCg");
        DontDestroyOnLoad(go);              // sigue visible al reiniciar el nivel
        instance = go.AddComponent<WatermarkDanerCg>();
    }

    void OnGUI()
    {
        if (style == null)
        {
            style = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.LowerRight
            };
        }

        style.fontSize = Mathf.Max(14, Mathf.RoundToInt(Screen.height * 0.05f));
        var area = new Rect(0, 0, Screen.width - 20, Screen.height - 15);

        // Sombra negra para que se lea sobre cualquier fondo
        style.normal.textColor = new Color(0f, 0f, 0f, 0.8f);
        GUI.Label(new Rect(area.x + 2, area.y + 2, area.width, area.height), Texto, style);

        // Texto blanco encima
        style.normal.textColor = new Color(1f, 1f, 1f, 0.9f);
        GUI.Label(area, Texto, style);
    }
}
