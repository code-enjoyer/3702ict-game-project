using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GGD
{
    public class ScoreTracker : Singleton<ScoreTracker>
    {
        [SerializeField] private TMP_Text _objectivesText;
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private TMP_Text _scoreText;

        public void UpdateObjectivesUI()
        {
            _objectivesText.text = $"Objectives: {LevelManager.Instance.ObjectivesDelivered}/{LevelManager.Instance.totalObjectives}";
        }

        public void UpdateTimeUI()
        {
            _timeText.text = $"Time: {LevelManager.Instance.timeTaken.ToString("0.0")} s";
        }

        public void UpdateScoreUI()
        {

        }
    }
}
