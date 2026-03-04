using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void IniciarJogo()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.QueueReset();

        SceneManager.LoadScene("Fase1");
    }
}