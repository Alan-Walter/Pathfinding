using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{
    private const int cameraMoveIndentSize = 5;  //  отступ от края экрана, в зоне которого начинается взаимодействие с перемещением
    private const float cameraMoveSize = 150.0f;  //  количество пунктов, на которое изменяется текущее положение/угол
    private const float cameraMoveY = 2.0f;  //  коэффициент увеличения скорости перемещения высоты камеры колёсиком мыши

    private Camera sceneCamera;  //  камера сцены

    private Vector3 cameraPosition;  //  позиция камеры
    private Vector3 mousePosition;  //  позиция указателя мыши
    private Vector3 cameraAngles;  //  углы камеры при нажатии средней клавиши мыши
    private Quaternion basicCameraRotation;  //  базовые (изначальные) углы вращения камеры
    private float cameraCosMove;  //  перемещение камеры с учётом косинуса угла камеры
    private float cameraSinMove;  //  перемещение камеры с учётом синуса угла камеры
    private SelectGameObject selectGameObject;  //  объект, отвечающий за выделение объектов(юнитов) на экране


    TerrainHeightMap terrainHeightMap;

    // Use this for initialization
    void Start ()
    {
        sceneCamera = Camera.main;  //  указываем на главную камеру сцены
        basicCameraRotation = sceneCamera.transform.rotation;  //  запоминаем углы камеры
        CalcMoveAngles();  //  вычисляем перемещение по сторонам относительно угла поворота камеры
        terrainHeightMap = TerrainHeightMap.Instance;
        selectGameObject = GetComponent<SelectGameObject>();  //  получаем объект SelectGameObject
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStates.gamePlayState != GamePlayState.Play || selectGameObject.IsSelectingObjects) return;
        mousePosition = Input.mousePosition;
        if (!Input.GetMouseButton(2))
        {
            cameraPosition = sceneCamera.transform.position;
            if (mousePosition.x < cameraMoveIndentSize || Input.GetAxis("Horizontal") < 0)
            {
                cameraPosition.x -= cameraCosMove * Time.deltaTime;
                cameraPosition.z += cameraSinMove * Time.deltaTime;
            }
            else if (mousePosition.x > Screen.width - cameraMoveIndentSize || Input.GetAxis("Horizontal") > 0)
            {
                cameraPosition.x += cameraCosMove * Time.deltaTime;
                cameraPosition.z -= cameraSinMove * Time.deltaTime;
            }
            if (mousePosition.y < cameraMoveIndentSize || Input.GetAxis("Vertical") < 0)
            {
                cameraPosition.x -= cameraSinMove * Time.deltaTime;
                cameraPosition.z -= cameraCosMove * Time.deltaTime;
            }
            else if (mousePosition.y > Screen.height - cameraMoveIndentSize || Input.GetAxis("Vertical") > 0)
            {
                cameraPosition.z += cameraCosMove * Time.deltaTime;
                cameraPosition.x += cameraSinMove * Time.deltaTime;
            }

            cameraPosition.y -= Input.mouseScrollDelta.y * cameraMoveSize * cameraMoveY * Time.deltaTime;
            cameraPosition.x = Mathf.Clamp(cameraPosition.x, 0, terrainHeightMap.Width);
            cameraPosition.z = Mathf.Clamp(cameraPosition.z, 0, terrainHeightMap.Length);
            cameraPosition.y = Mathf.Clamp(cameraPosition.y, GameConstants.CameraMinHeight, GameConstants.CameraMaxHeight);
            sceneCamera.transform.position = cameraPosition;
        }
        else
        {
            sceneCamera.transform.Rotate(0.0f, cameraMoveSize * Input.GetAxis("Mouse X") * Time.deltaTime, 0.0f, Space.World);
            //  выполняем поворот камеры
            CalcMoveAngles();
        }
        if(Input.GetMouseButtonDown(2))
        {
            cameraAngles = sceneCamera.transform.eulerAngles;
        }
        if (Input.GetMouseButtonUp(2) && cameraAngles == sceneCamera.transform.eulerAngles)  
            //  возвращаем стандартные значения при нажатии средней клавиши мыши
        {
            sceneCamera.transform.rotation = basicCameraRotation;
            CalcMoveAngles();
        }
    }

    private void CalcMoveAngles()
    {
        cameraCosMove = Mathf.Cos(sceneCamera.transform.eulerAngles.y * Mathf.Deg2Rad) * cameraMoveSize;  //  вычисляем синус и косинусы
        cameraSinMove = Mathf.Sin(sceneCamera.transform.eulerAngles.y * Mathf.Deg2Rad) * cameraMoveSize;
    }
}
