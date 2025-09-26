using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (ScoreManager.Instance != null)
        {
            scoreText.text = "Score: " + ScoreManager.Instance.CurrentScore;
        }
    }
}