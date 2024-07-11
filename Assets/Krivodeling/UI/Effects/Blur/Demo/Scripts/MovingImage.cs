using System.Collections;
using UnityEngine;

namespace Krivodeling.UI.Effects.Examples
{
    public class MovingImage : MonoBehaviour
    {
        [SerializeField]
        private float _minX = -570f;
        [SerializeField]
        private float _maxX = 570f;
        [SerializeField]
        private float _minY = -300f;
        [SerializeField]
        private float _maxY = 300f;
        [Space, SerializeField]
        private float _minTime = 3f;
        [SerializeField]
        private float _maxTime = 10f;
        [Space, SerializeField]
        private float _speed = 0.5f;

        private RectTransform _transform;
        private Vector2 _target;

        private void Awake()
        {
            _transform = GetComponent<RectTransform>();

            StartCoroutine(SetTargetCoroutine());
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            _transform.anchoredPosition = Vector2.Lerp(_transform.anchoredPosition, _target, _speed * Time.deltaTime);
        }

        private IEnumerator SetTargetCoroutine()
        {
            _target = GetRandomTarget();

            yield return GetRandomWait();

            StartCoroutine(SetTargetCoroutine());
        }

        private Vector2 GetRandomTarget()
        {
            return new Vector2
            {
                x = Random.Range(_minX, _maxX),
                y = Random.Range(_minY, _maxY)
            };
        }

        private WaitForSeconds GetRandomWait()
        {
            float time = Random.Range(_minTime, _maxTime);

            return new WaitForSeconds(time);
        }
    }
}
