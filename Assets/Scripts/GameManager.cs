using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Configuração")]
    public int vidasIniciais = 3;
    public int pontosPorBrick = 100;

    // UI (será re-ligado automaticamente quando trocar de cena)
    private TMP_Text scoreText;
    private TMP_Text livesText;

    private int score;
    private int vidas;
    private int bricksRestantes;

    // quando voltar do menu/tela final e entrar na Fase1, reseta score/vidas
    private bool resetPending = true;

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
        // Religa HUD quando entrar em fases
        if (scene.name == "Fase1" || scene.name == "Fase2")
        {
            BindHUDByName();     // encontra ScoreText e LivesText pela Hierarchy
            RecountBricks();     // conta bricks destrutíveis dessa fase

            if (scene.name == "Fase1" && resetPending)
            {
                // reset de uma nova "run" (menu/restart)
                score = 0;
                vidas = vidasIniciais;
                resetPending = false;
            }

            AtualizarUI();
        }
        else
        {
            // MenuInicial ou TelaFinal -> não precisa de HUD
            scoreText = null;
            livesText = null;
        }
    }

    // Chame isso antes de carregar a Fase1 a partir do Menu ou Reiniciar
    public void QueueReset()
    {
        resetPending = true;
    }

    void BindHUDByName()
    {
        // Procura qualquer TMP_Text carregado e pega pelos nomes
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
            // conta apenas os destrutíveis
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
            SceneManager.LoadScene("Fase2");
        else
            SceneManager.LoadScene("TelaFinal");
    }

    void GameOver()
    {
        SceneManager.LoadScene("TelaFinal");
    }

    public int GetScore() => score;
    public int GetLives() => vidas;
}