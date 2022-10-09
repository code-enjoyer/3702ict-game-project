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

        public void UpdateObjectivesUI(int delivered, int total)
        {
            _objectivesText.text = $"Packages: {delivered}/{total}";
        }

        public void UpdateTimeUI(float timeTaken)
        {
            _timeText.text = $"Time: {timeTaken : 0.0} s";
        }

        public void UpdateScoreUI()
        {
            // TODO: Finish score system first
        }
    }
}
