using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinalScreenController : MonoBehaviour
{
    public TMP_Text finalScoreText;
    public TMP_Text titleText;

    public void Start()
    {
        // Mostra score final
        if (finalScoreText != null && GameManager.Instance != null)
        {
            // como score é privado no GameManager, a solução simples é:
            // colocar um getter público (vou te mostrar já já)
            finalScoreText.text = "Score: " + GameManager.Instance.GetScore();
        }

        // Se quiser diferenciar vitória/derrota, depois fazemos com um "estado"
        if (titleText != null)
        {
            titleText.text = "FIM DE JOGO";
        }
    }

    public void Restart()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.QueueReset();

        SceneManager.LoadScene("Fase1");
    }
}