using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CameraFollowObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _playerTransform;

    [Header("Flip Rotation Stats")]
    [SerializeField] private float _flipYRotationTime = 0.5f;

    private Coroutine _turnCoroutine;

    [SerializeField] private PlayerController _player;
    private bool _isFacingRight;

    private void Awake()
    {
        _player = _playerTransform.gameObject.GetComponent<PlayerController>();
        _isFacingRight = _player.isFacingRight;
    }

    private void Update()
    {
        transform.position = _playerTransform.position;

        if (_isFacingRight != _player.isFacingRight)
        {
            _isFacingRight = _player.isFacingRight;

            if (_turnCoroutine != null)
                StopCoroutine(_turnCoroutine);

            _turnCoroutine = StartCoroutine(FlipYLerp());
        }
    }

    public void CallTurn()
    {
        _turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < _flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;

            //lerp the y rotation
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / _flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        //_isFacingRight = !_isFacingRight;

        if (_isFacingRight) return 0f;
        else return 180f;
    }
}
