using UnityEngine;

public class CloseGame : MonoBehaviour
{
    void Update()
    {
        // Check if the ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // If running in the editor, stop playing
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // Close the application
            Application.Quit();
#endif
        }
    }
}
