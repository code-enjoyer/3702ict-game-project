using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UltEvents;
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
        public int totalObjectives;
        public int objectivesRemaining;
        private int _objectivesDelivered;
        public int ObjectivesDelivered
        {
            get => _objectivesDelivered;
            set
            {
                _objectivesDelivered = value;
                ScoreTracker.Instance?.UpdateObjectivesUI();
            }
        }
        public float timeTaken;

        public UltEvent OnBeforeLevelUnloaded;
        public UltEvent OnLevelLoaded;

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
            FindObjectives();
            FindSpawnPoint();

            OnLevelLoaded += FindObjectives;
            OnLevelLoaded += FindSpawnPoint;
            OnLevelLoaded += () => { timeTaken = 0f; };
        }

        // DEBUG
        private Queue<IEnumerator> _currentActions = new();
        // DEBUG
        private Coroutine _actionExecutionCo;

        // DEBUG: Testing chaining coroutines together to create dynamic game flow. Will hopefully allow for logic execution to wait for animation and/or user input.
        private IEnumerator ExecuteCurrentActionsCo()
        {
            while (_currentActions.Count > 0)
            {
                yield return StartCoroutine(_currentActions.Dequeue());
            }

            _actionExecutionCo = null;
        }

        public void EnqueueCoroutine(IEnumerator routine)
        {
            _currentActions.Enqueue(routine);
        }

        private void Update()
        {
            if (GameManager.IsInitialized && GameManager.Instance.CurrentState == GameManager.Instance.PlayingState)
            {
                timeTaken += Time.deltaTime;
                ScoreTracker.Instance?.UpdateTimeUI();
            }

            // DEBUG
            if (_actionExecutionCo == null && _currentActions.Count > 0)
            {
                _actionExecutionCo = StartCoroutine(ExecuteCurrentActionsCo());
            }
        }

        private void FindObjectives()
        {
            totalObjectives = FindObjectsOfType<Objective>().Length;
            objectivesRemaining = totalObjectives;
        }

        private void FindSpawnPoint()
        {
            _spawnPoint = FindObjectOfType<SpawnPoint>();
            if (_spawnPoint == null && GameManager.Instance.Player != null)
            {
                _spawnPoint = new GameObject("Spawn Point").AddComponent<SpawnPoint>();
                _spawnPoint.transform.position = GameManager.Instance.Player.transform.position;
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
                    return _spawnPoint.transform.position;
            }
        }

        // NOTE: Not sure if the non-static methods are needed or would be used...
        public static void EndLevelStatic()
        {
            Instance.EndLevel();
        }

        public void EndLevel()
        {
            // GameState to paused
            // Cleanup / stop any things that need stopping
            // Display end level UI
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

            OnBeforeLevelUnloaded.Invoke();

            AsyncOperation scene = SceneManager.LoadSceneAsync("Loading");
            
            // LOAD SCREEN
            while (!scene.isDone)
            {
                yield return null;
            }

            // LOAD LEVEL CO
            yield return StartCoroutine(LoadLevelCo(levelName));

            OnLevelLoaded.Invoke();

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
