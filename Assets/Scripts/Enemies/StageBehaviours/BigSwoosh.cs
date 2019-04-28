using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwooshStage
{
    SideToSide,
    JumpToMiddle,
    Still,
}

public class BigSwoosh : IEnemyState
{
    private GameObject _gameObject;
    private EnemyController _controller;
    private SwooshStage _stage;

    private static float _sideToSideSpeed = 0.3f;
    private static int _sideToSideTurns = 2;

    private static float _jumpLerp = 0.1f;
    private static float _jumpSnap = 0.05f;
    private static float _stillDuration = 5.5f;
    private static string _bulletName = BulletTypes.Simple;

    private IEnumerator _currentBehaviour;

    private Vector2 _jumpPosition;
    private float _sideSize;

    private float _t;

    /* === interface methods === */
    public void Start(GameObject gameObject, EnemyController controller)
    {
        _gameObject = gameObject;
        _controller = controller;

        _jumpPosition = gameObject.transform.position;
        _sideSize = Game.Instance.PixelPerfectCamera.OrthoScale;
    }

    public void Update()
    {
        if (_currentBehaviour == null)
        {
            _t = 0f;
            switch (_stage)
            {
                case SwooshStage.SideToSide:
                    _currentBehaviour = DoSideToSide();
                    break;
                case SwooshStage.JumpToMiddle:
                    _currentBehaviour = DoJumpToMiddle();
                    break;
                case SwooshStage.Still:
                    _currentBehaviour = DoStill();
                    break;
                default:
                    _currentBehaviour = DoJumpToMiddle();
                    break;
            }

            _controller.StartCoroutine(_currentBehaviour);
        }
        else
        {
            // update positions
            switch (_stage)
            {
                case SwooshStage.SideToSide:
                    _t += Time.deltaTime * _sideToSideSpeed;
                    _gameObject.transform.position = _jumpPosition + new Vector2(Mathf.Sin(_t * Mathf.PI * 2f) * _sideSize, 0);
                    if (_t >= (float)_sideToSideTurns)
                    {
                        _stage = SwooshStage.JumpToMiddle;
                        _controller.StopCoroutine(_currentBehaviour);
                        _currentBehaviour = null;
                    }
                    break;

                case SwooshStage.JumpToMiddle:
                    _gameObject.transform.position =
                        Vector3.Lerp(
                            _gameObject.transform.position,
                            _jumpPosition,
                            _jumpLerp
                        );

                    Vector2 diff = (Vector2)_gameObject.transform.position - _jumpPosition;

                    if (diff.magnitude <= _jumpSnap)
                    {
                        _gameObject.transform.position = _jumpPosition;
                        _stage = SwooshStage.Still;
                        _controller.StopCoroutine(_currentBehaviour);
                        _currentBehaviour = null;
                    }

                    break;

                case SwooshStage.Still:
                    _t += Time.deltaTime;
                    if (_t >= _stillDuration)
                    {
                        _stage = SwooshStage.SideToSide;
                        _controller.StopCoroutine(_currentBehaviour);
                        _currentBehaviour = null;
                    }
                    break;
            }
        }
    }

    public void End()
    {

    }

    private IEnumerator DoSideToSide()
    {
        while (true)
        {
            for (int i = 0; i < 10; i++)
            {
                _gameObject.Shoot(_bulletName, Vector2.zero, 0f);
                _gameObject.Shoot(_bulletName, Vector2.zero, 180f);
                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator DoJumpToMiddle()
    {
        yield return null;
    }

    private IEnumerator DoStill()
    {
        yield return new WaitForSeconds(0.3f);
        while(true)
        {
            float angle = 90f * Mathf.Sin(_t * Mathf.PI);
            _gameObject.Shoot(_bulletName, Vector2.zero, angle);
            _gameObject.Shoot(_bulletName, Vector2.zero, angle + 180f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
