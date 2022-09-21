using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private RectTransform _transitionRoot;
        [SerializeField] private Image _transitionMask;
        [SerializeField] private Image _transitionBackground;

        protected override void Awake()
        {
            base.Awake();

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

        // TODO: This needs to become async or a coroutine
        public void GoToLevel(string levelName, bool playAnimation = true)
        {
            GameManager.Instance.SetState(GameManager.Instance.PausedState);

            TransitionOut();

            SceneManager.LoadScene(levelName);

            TransitionIn();
        }

        [ContextMenu("Transition Out")]
        private void TransitionOut()
        {
            _transitionRoot.gameObject.SetActive(true);

            // Background alpha fade
            Color c = _transitionBackground.color;
            c.a = 0f;
            _transitionBackground.color = c;

            _transitionBackground.DOFade(1f, _transitionDuration * 0.35f);

            // Mask scale and rotate
            _transitionMask.rectTransform.localScale = Vector3.one;
            Vector3 r = _transitionMask.rectTransform.localRotation.eulerAngles;
            r.z = Random.Range(-180f, 180f);
            _transitionMask.rectTransform.localRotation = Quaternion.Euler(r);

            _transitionMask.rectTransform.DOBlendableLocalRotateBy(new Vector3(0f, 0f, -540f), _transitionDuration, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
            _transitionMask.rectTransform.DOScale(0f, _transitionDuration).SetEase(Ease.Linear).OnComplete(() => {
                // TODO: Swap to next scene or loading screen
                TransitionIn();
            });
        }

        [ContextMenu("Transition In")]
        private void TransitionIn()
        {
            _transitionRoot.gameObject.SetActive(true);

            // Background alpha fade
            Color c = _transitionBackground.color;
            c.a = 1f;
            _transitionBackground.color = c;

            _transitionBackground.DOFade(0f, _transitionDuration * 0.5f).SetDelay(_transitionDuration * 0.5f);

            // Mask scale and rotate
            _transitionMask.rectTransform.localScale = Vector3.zero;
            Vector3 r = _transitionMask.rectTransform.localRotation.eulerAngles;
            r.z = Random.Range(-180f, 180f);
            _transitionMask.rectTransform.localRotation = Quaternion.Euler(r);

            _transitionMask.rectTransform.DOBlendableLocalRotateBy(new Vector3(0f, 0f, 540f), _transitionDuration, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
            _transitionMask.rectTransform.DOScale(1.5f, _transitionDuration).SetEase(Ease.Linear).OnComplete(() => {
                
            });
        }
    }

    // TODO: Make a ScriptableObject for storing the data for each level?
}
