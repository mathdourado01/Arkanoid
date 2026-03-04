using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum EndState { None, Win, Lose }

    [Header("Configuração")]
    public int vidasIniciais = 3;
    public int pontosPorBrick = 100;


    private TMP_Text scoreText;
    private TMP_Text livesText;

    private int score;
    private int vidas;
    private int bricksRestantes;

   
    private bool resetPending = true;

    
    private EndState endState = EndState.None;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (scene.name == "Fase1" || scene.name == "Fase2")
        {
            BindHUDByName();     
            RecountBricks();     

            
            if (scene.name == "Fase1" && resetPending)
            {
                score = 0;
                vidas = vidasIniciais;
                resetPending = false;
                endState = EndState.None;
            }

            AtualizarUI();
        }
        else
        {
            
            scoreText = null;
            livesText = null;
        }
    }

    
    public void QueueReset()
    {
        resetPending = true;
        endState = EndState.None;
    }

    void BindHUDByName()
    {
        TMP_Text[] texts = FindObjectsByType<TMP_Text>(FindObjectsSortMode.None);
        scoreText = null;
        livesText = null;

        foreach (var t in texts)
        {
            if (t.name == "ScoreText") scoreText = t;
            else if (t.name == "LivesText") livesText = t;
        }
    }

    void RecountBricks()
    {
        bricksRestantes = 0;

        GameObject[] bricks = GameObject.FindGameObjectsWithTag("Brick");
        foreach (GameObject b in bricks)
        {
            Brick br = b.GetComponent<Brick>();
            
            if (br == null || br.type != BrickType.Indestructible)
                bricksRestantes++;
        }
    }

    public void BrickDestruido()
    {
        score += pontosPorBrick;
        bricksRestantes--;

        AtualizarUI();

        if (bricksRestantes <= 0)
            VencerFase();
    }

    public void PerderVida()
    {
        vidas--;
        AtualizarUI();

        if (vidas <= 0)
            GameOver();
    }

    public void GanharVida(int qtd = 1)
    {
        vidas += qtd;
        AtualizarUI();
    }

    void AtualizarUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (livesText != null)
            livesText.text = "Vidas: " + vidas;
    }

    void VencerFase()
    {
        string cena = SceneManager.GetActiveScene().name;

        if (cena == "Fase1")
        {
            SceneManager.LoadScene("Fase2");
        }
        else
        {
            endState = EndState.Win;
            SceneManager.LoadScene("TelaFinal");
        }
    }

    void GameOver()
    {
        endState = EndState.Lose;
        SceneManager.LoadScene("TelaFinal");
    }

    
    public int GetScore() => score;
    public int GetLives() => vidas;
    public EndState GetEndState() => endState;
}