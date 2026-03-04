using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinalScreenController : MonoBehaviour
{
    public TMP_Text finalScoreText;
    public TMP_Text titleText;

    void Start()
        {
            if (GameManager.Instance != null)
            {
                
                if (finalScoreText != null)
                    finalScoreText.text = "Score: " + GameManager.Instance.GetScore();

                
                if (titleText != null)
                {
                    var state = GameManager.Instance.GetEndState();
                    if (state == GameManager.EndState.Win)
                        titleText.text = "VOCÊ VENCEU!";
                    else if (state == GameManager.EndState.Lose)
                        titleText.text = "GAME OVER, VOCÊ PERDEUUU!!";
                    else
                        titleText.text = "FIM";
                }
            }
        }

    public void Restart()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.QueueReset();

        SceneManager.LoadScene("Fase1");
    }
}