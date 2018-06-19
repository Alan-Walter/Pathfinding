using UnityEngine;
using System.Collections;

/// <summary>
/// Класс, отвечающий за перемещение камеры
/// </summary>
public class CameraMove : MonoBehaviour {
    #region Поля
    private const int cameraMoveIndentSize = 5;  //  отступ от края экрана, в зоне которого начинается взаимодействие с перемещением
    private const float cameraMoveSize = 150.0f;  //  количество пунктов, на которое изменяется текущее положение/угол
    private const float cameraRotatationSize = 60;  //  кол-во пунктов, на которое изменится угол поворота камеры
    private const float cameraMoveY = 2.0f;  //  коэффициент увеличения скорости перемещения высоты камеры колёсиком мыши

    private float CameraMoveCoef
    {
        get
        {
            return sceneCamera.transform.position.y * cameraMoveSize / GameConstants.CameraMaxHeight;
        }
    }  //  Коэффициент скорости перемещения камеры в зависимости от высоты

    private Camera sceneCamera;  //  камера сцены

    private Vector3 cameraPosition;  //  позиция камеры
    private Vector3 mousePosition;  //  позиция указателя мыши
    private Vector3 cameraAngles;  //  углы камеры при нажатии средней клавиши мыши
    private Quaternion basicCameraRotation;  //  базовые (изначальные) углы вращения камеры
    private float cameraCosMove;  //  перемещение камеры с учётом косинуса угла камеры
    private float cameraSinMove;  //  перемещение камеры с учётом синуса угла камеры
    private SelectGameObject selectGameObject;  //  объект, отвечающий за выделение объектов(юнитов) на экране
    #endregion
    // Use this for initialization
    void Start () {
        sceneCamera = Camera.main;  //  указываем на главную камеру сцены
        basicCameraRotation = sceneCamera.transform.rotation;  //  запоминаем углы камеры
        CalcMoveAngles();  //  вычисляем перемещение по сторонам относительно угла поворота камеры
        selectGameObject = GetComponent<SelectGameObject>();  //  получаем объект SelectGameObject
        GameParams.GamePlayState = GamePlayState.Play;  //  устанавливаем состояние игры на Play
    }

    // Update is called once per frame
    void Update() {
        if (GameParams.CameraState == CameraStates.Freeze || selectGameObject.IsSelectingObjects) return;
        mousePosition = Input.mousePosition;
        if (!Input.GetMouseButton(2))
        {
            cameraPosition = sceneCamera.transform.position;
            if (mousePosition.x < cameraMoveIndentSize || Input.GetAxis("Horizontal") < 0)
            {
                cameraPosition.x -= cameraCosMove * Time.deltaTime * CameraMoveCoef;
                cameraPosition.z += cameraSinMove * Time.deltaTime * CameraMoveCoef;
            }
            else if (mousePosition.x > Screen.width - cameraMoveIndentSize || Input.GetAxis("Horizontal") > 0)
            {
                cameraPosition.x += cameraCosMove * Time.deltaTime * CameraMoveCoef;
                cameraPosition.z -= cameraSinMove * Time.deltaTime * CameraMoveCoef;
            }
            if (mousePosition.y < cameraMoveIndentSize || Input.GetAxis("Vertical") < 0)
            {
                cameraPosition.x -= cameraSinMove * Time.deltaTime * CameraMoveCoef;
                cameraPosition.z -= cameraCosMove * Time.deltaTime * CameraMoveCoef;
            }
            else if (mousePosition.y > Screen.height - cameraMoveIndentSize || Input.GetAxis("Vertical") > 0)
            {
                cameraPosition.z += cameraCosMove * Time.deltaTime * CameraMoveCoef;
                cameraPosition.x += cameraSinMove * Time.deltaTime * CameraMoveCoef;
            }

            cameraPosition.y -= Input.mouseScrollDelta.y * CameraMoveCoef * cameraMoveY * Time.deltaTime;
            cameraPosition.x = Mathf.Clamp(cameraPosition.x, 0, GameParams.Width);
            cameraPosition.z = Mathf.Clamp(cameraPosition.z, 0, GameParams.Length);
            cameraPosition.y = Mathf.Clamp(cameraPosition.y, GameConstants.CameraMinHeight, GameConstants.CameraMaxHeight);
            sceneCamera.transform.position = cameraPosition;
        }
        else
        {
            sceneCamera.transform.Rotate(0.0f, cameraRotatationSize * Input.GetAxis("Mouse X") * Time.deltaTime, 0.0f, Space.World);
            //  выполняем поворот камеры
            CalcMoveAngles();
        }
        if(Input.GetMouseButtonDown(2))
            cameraAngles = sceneCamera.transform.eulerAngles;
        if (Input.GetMouseButtonUp(2) && cameraAngles == sceneCamera.transform.eulerAngles)  
            //  возвращаем стандартные значения при нажатии средней клавиши мыши
        {
            sceneCamera.transform.rotation = basicCameraRotation;
            CalcMoveAngles();
            sceneCamera.transform.position = new Vector3(sceneCamera.transform.position.x, GameConstants.CameraMaxHeight,
                sceneCamera.transform.position.z);
        }
    }

    /// <summary>
    /// Функция, вычисляющая синус и косинус для правильного перемещения камеры над полем
    /// </summary>
    private void CalcMoveAngles() {
        cameraCosMove = Mathf.Cos(sceneCamera.transform.eulerAngles.y * Mathf.Deg2Rad);
        cameraSinMove = Mathf.Sin(sceneCamera.transform.eulerAngles.y * Mathf.Deg2Rad);
    }
}
