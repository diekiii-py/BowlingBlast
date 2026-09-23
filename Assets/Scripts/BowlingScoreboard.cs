using TMPro;
using UnityEngine;

public class BowlingScoreboard : MonoBehaviour
{
    public BowlingScoreManager scoreManager;
    public TMP_Text scoreboardText;
    public float refreshInterval = 0.1f;

    private float refreshTimer;

    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        refreshTimer -= Time.deltaTime;
        if (refreshTimer > 0f)
            return;

        refreshTimer = refreshInterval;
        Refresh();
    }

    public void Refresh()
    {
        if (scoreManager == null || scoreboardText == null)
            return;

        scoreboardText.text = scoreManager.GetScoreboardText();
    }
}
