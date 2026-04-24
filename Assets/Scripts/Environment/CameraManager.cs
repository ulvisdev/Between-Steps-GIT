// using UnityEngine;
// using Cinemachine;
// using System.Collections;

// public class CameraManager : MonoBehaviour
// {
//     public static CameraManager instance;
//     [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;

//     [Header("Controls for lerping the Y Damping during player jump/fall")]
//     [SerializeField] private float _fallPanAmount = 0.25f;
//     [SerializeField] private float _fallYPanTime = 0.35f;
//     public float _fallSpeedYDampingChangeThreshold = -15f;

//     public bool IsLerpingYDamping { get; private set; }
//     public bool LerpedFromPlayerFalling { get; set; }

//     private CinemachineVirtualCamera _currentCamera;
//     private CinemachinePositionTransposer _positionTransposer;
    
//     private float _normYPanAmount;

//     private Coroutine _lerpYPanCoroutine;

//     private void Awake()
//     {
//         if (instance == null)
//         {
//             instance = this;
//         }

//         for (int i = 0; i < _allVirtualCameras.Length; i++)
//         {
//             //set the current active camera
//             _currentCamera = _allVirtualCameras[i];

//             //set the position transposer
//             _positionTransposer = _currentCamera.GetCinemachineComponent<CinemachinePositionComposer>();
//         }
//     }

//     #region Lerp the Y Damping

//     public void LerpYDamping(bool isPlayerFalling)
//     {
//         _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
//     }

//     private IEnumerator LerpYAction(bool isPlayerFalling)
//     {
//         isLerpingYDamping = true;

//         //grab the starting damping amount
//         float startDampAmount = _framingTransposer.m_YDamping;
//         float endDampAmount = 0f;

//         if (isPlayerFalling)
//         {
//             endDampAmount = _fallPanAmount;
//             LerpedFromPlayerFalling = true;
//         }
//         else
//         {
//             endDampAmount = _normYPanAmount;
//         }

//         float elapsedTime = 0f;
//         while (elapsedTime < _fallYPanTime)
//         {
//             elapsedTime += Time.deltaTime;
            
//             float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / _fallYPanTime));
//             _framingTransposer.m_YDamping = lerpedPanAmount;

//             yield return null;
//         }

//         isLerpingYDamping = true;
//     }
// }
