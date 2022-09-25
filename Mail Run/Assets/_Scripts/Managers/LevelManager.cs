using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GGD
{
    /// <summary>
    /// A singleton responsible for storing data and providing functions pertaining to the current level.
    /// One level manager should be present in each level.
    /// </summary>
    public class LevelManager : Singleton<LevelManager>
    {
        [SerializeField] private SpawnPoint _spawnPoint;

        [Header("Level Transitions")]
        [SerializeField] private float _transitionDuration = 3f;
        [SerializeField] private RectTransform _transitionScreen;
        [SerializeField] private Image _transitionMask;
        [SerializeField] private Image _transitionBackground;
        [SerializeField] private RectTransform _loadScreen;
        [SerializeField] private Image _progressFill;
        public int objectivesRemaining;
        public int objectivesDelivered;


        //private delegate bool BreadCrumbMethod();
        //private List<BreadCrumbMethod> _onLevelLoadChain = new();
        // e.g: _onLevelLoadChain.Add(() => { return true; });
        // How to make this work with functions? e.g. _onLevelLoadChain.Add(SomeFunction)?

        protected override void Awake()
        {
            base.Awake();

            // TODO: Because this is persistant, will only get the spawn point from the first level, will need to re-get after each scene load
            if (_spawnPoint == null)
                _spawnPoint = FindObjectOfType<SpawnPoint>();
        }

        private void Start()
        {
            if (_spawnPoint == null && GameManager.IsInitialized && GameManager.Instance.Player != null)
            {
                _spawnPoint = new GameObject("Spawn Point").AddComponent<SpawnPoint>();
                _spawnPoint.transform.position = GameManager.Instance.Player.transform.position;
            }

            FindObjectives();
        }

        void FindObjectives()
        {
            objectivesRemaining = FindObjectsOfType<Objective>().Length;
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
                    return _spawnPoint.transform.position;
            }
        }

        public static void GoToLevelStatic(string levelName, bool animateOut, bool animateIn)
        {
            Instance.GoToLevel(levelName, animateOut, animateIn);
        }

        public void GoToLevel(string levelName, bool animateOut, bool animateIn)
        {
            StartCoroutine(GoToLevelCo(levelName, animateOut, animateIn));
        }

        public void GoToLevel(string levelName)
        {
            GoToLevel(levelName, true, true);
        }

        public void GoToLevelNoFadeOut(string levelName)
        {
            GoToLevel(levelName, false, true);
        }

        public void GoToLevelNoFadeIn(string levelName)
        {
            GoToLevel(levelName, true, false);
        }

        public void GoToLevelNoFade(string levelName)
        {
            GoToLevel(levelName, false, false);
        }

        // NOTE: Not sure if this is even needed...
        public void RestartLevel()
        {
            GoToLevel(SceneManager.GetActiveScene().name);
        }

        private IEnumerator GoToLevelCo(string levelName, bool animateOut = true, bool animateIn = true)
        {
            // Set GameManager state to paused

            // TRANSITION OUT CO
            if (animateOut)
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
            if (animateIn)
                yield return StartCoroutine(TransitionInCo());
            else
                _transitionBackground.gameObject.SetActive(false);

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
            _transitionMask.rectTransform.localScale = Vector3.one * 1.5f;
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
            _transitionScreen.gameObject.SetActive(true);
            _transitionMask.rectTransform.localScale = Vector3.zero;
            Color c = _transitionBackground.color;
            c.a = 1f;
            _transitionBackground.color = c;

            // Play transition animation
            // Fade out background/overlay (but on the tail end of the animation/effect)
            _transitionBackground.DOFade(0f, _transitionDuration * 0.2f).SetDelay(_transitionDuration * 0.8f);

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
    }

    // TODO: Make a ScriptableObject for storing the data for each level?
}
