using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GGD
{
    /// <summary>
    /// A singleton responsible for storing data and providing functions pertaining to the current level.
    /// One level manager should be present in each level.
    /// </summary>
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private Transform _spawnPoint;

        [Header("Level Transitions")]
        [SerializeField] private float _transitionDuration = 3f;
        [SerializeField] private RectTransform _transitionScreen;
        [SerializeField] private Image _transitionMask;
        [SerializeField] private Image _transitionBackground;
        [SerializeField] private RectTransform _loadScreen;
        [SerializeField] private Image _progressFill;

        protected override void Awake()
        {
            base.Awake();

            // TODO: Because this is persistant, will only get the spawn point from the first level, will need to re-get after each scene load
            if (_spawnPoint == null)
            {
                GameObject go = GameObject.FindGameObjectWithTag("SpawnPoint");
                if (go != null)
                    _spawnPoint = go.transform;
            }
        }

        public Vector3 SpawnLocation
        {
            get
            {
                if (_spawnPoint == null)
                {
                    Debug.LogWarning("[LevelManger] No spawn point set, returning the manager's position.", gameObject);
                    return transform.position;
                }
                else
                    return _spawnPoint.position;
            }
        }

        public void GoToLevel(string levelName)
        {
            StartCoroutine(GoToLevelCo(levelName));
        }

        // NOTE: Not sure if this is even needed...
        public void RestartLevel()
        {

        }

        private IEnumerator RestartLevelCo()
        {
            // Set GameManager state to paused

            // TRANSITION OUT CO
            yield return StartCoroutine(TransitionOutCo());

            // TRANSITION IN CO
            yield return StartCoroutine(TransitionInCo());

            // Set GameManager state to playing
        }

        private IEnumerator GoToLevelCo(string levelName)
        {
            // Set GameManager state to paused

            // TRANSITION OUT CO
            yield return StartCoroutine(TransitionOutCo());

            AsyncOperation scene = SceneManager.LoadSceneAsync("Loading");
            
            // LOAD SCREEN
            while (!scene.isDone)
            {
                yield return null;
            }

            // LOAD LEVEL CO
            yield return StartCoroutine(LoadLevelCo(levelName));

            // TRANSITION IN CO
            yield return StartCoroutine(TransitionInCo());

            // Set GameManager state to playing
        }

        private IEnumerator LoadLevelCo(string levelName)
        {
            WaitForSecondsRealtime wait = new WaitForSecondsRealtime(0.1f);

            // Enable and reset loading display
            _loadScreen.gameObject.SetActive(true);
            _progressFill.fillAmount = 0f;

            yield return wait;

            // Start loading new scene
            AsyncOperation scene = SceneManager.LoadSceneAsync(levelName);
            scene.allowSceneActivation = false;

            // Update progress bar
            while (_progressFill.fillAmount < 0.999f)
            {
                yield return null;
                _progressFill.fillAmount = Mathf.Lerp(_progressFill.fillAmount, scene.progress / 0.9f, Time.deltaTime * 10f);
            }

            yield return wait;

            // Hide loading display
            _loadScreen.gameObject.SetActive(false);

            // Let the scene load
            scene.allowSceneActivation = true;
        }

        private IEnumerator TransitionOutCo()
        {
            // Enable and reset transition display
            _transitionScreen.gameObject.SetActive(true);
            Color c = _transitionBackground.color;
            c.a = 0f;
            _transitionBackground.color = c;

            // Play transition animation
            // Fade in background/overlay
            _transitionBackground.DOFade(1f, _transitionDuration * 0.35f);

            // Set random initial rotation for mask
            _transitionMask.rectTransform.localScale = Vector3.one;
            Vector3 r = _transitionMask.rectTransform.localRotation.eulerAngles;
            r.z = Random.Range(-180f, 180f);
            _transitionMask.rectTransform.localRotation = Quaternion.Euler(r);

            // Scale and rotate mask
            _transitionMask.rectTransform.DOBlendableLocalRotateBy(new Vector3(0f, 0f, -540f), _transitionDuration, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
            yield return _transitionMask.rectTransform.DOScale(0f, _transitionDuration).SetEase(Ease.Linear).WaitForCompletion();
        }

        private IEnumerator TransitionInCo()
        {
            // Reset transition display
            Color c = _transitionBackground.color;
            c.a = 1f;
            _transitionBackground.color = c;

            // Play transition animation
            // Fade out background/overlay (but on the tail end of the animation/effect)
            _transitionBackground.DOFade(0f, _transitionDuration * 0.5f).SetDelay(_transitionDuration * 0.5f);

            // Set random initial rotation for mask
            _transitionMask.rectTransform.localScale = Vector3.zero;
            Vector3 r = _transitionMask.rectTransform.localRotation.eulerAngles;
            r.z = Random.Range(-180f, 180f);
            _transitionMask.rectTransform.localRotation = Quaternion.Euler(r);

            // Scale and rotate mask
            _transitionMask.rectTransform.DOBlendableLocalRotateBy(new Vector3(0f, 0f, 540f), _transitionDuration, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
            yield return _transitionMask.rectTransform.DOScale(1.5f, _transitionDuration).SetEase(Ease.Linear).WaitForCompletion();

            _transitionScreen.gameObject.SetActive(false);
        }

        [ContextMenu("Test")]
        private void Test()
        {
            GoToLevel("Level02");
        }
    }

    // TODO: Make a ScriptableObject for storing the data for each level?
}
