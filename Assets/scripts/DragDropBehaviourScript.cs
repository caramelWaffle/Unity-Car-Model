using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DragDropBehaviourScript : MonoBehaviour
{
    //Initialize Variables
    public GameObject getTarget;
    public bool isMouseDragging = false;
    Vector3 offsetValue;
    Vector3 positionOfScreen;
    private bool isRotate = true;
    private float sensitive = 1f;

    public Quaternion originalRotationValue; // declare this as a Quaternion
    float rotationResetSpeed = 1.0f;
    public Vector3 _OrignalScale;

    public Text taskText;
    public InputField InputName;
    public Button btnSave;
    public Text statusText;
    public Text saveStatus;

    bool isAssembly = false;
    bool isComplete = false;
    bool isSave = false;
    bool isSwap = false;

    List<MouseData> dataList;
    public MouseDataManager mouseDataManager;
    float logicaaTime = 0.0f;
    public GameObject wheel2;

    void Start()
    {

        statusText.text = "Incomplete";
        statusText.color = Color.red;
        saveStatus.gameObject.SetActive(false);
        isComplete = false;

        InputName.gameObject.SetActive(false);
        btnSave.gameObject.SetActive(false);


        btnSave.onClick.AddListener(delegate () {
            string fileName = InputName.text;
            Debug.Log(fileName);
            string path = mouseDataManager.save(fileName, dataList);
            saveStatus.text = "saved at " + path;
            saveStatus.gameObject.SetActive(true);
            isSave = true;

        });


        originalRotationValue = transform.rotation; // save the initial rotation
        _OrignalScale=transform.localScale;

        //this.transform.position = _CoreWheel.GetComponent<CoreWheel>()._Wheel.transform.position;
        //this.transform.rotation = _CoreWheel.GetComponent<CoreWheel>()._Wheel.transform.rotation;

        string task = Scenes.getParam("task");

        if (task == "1")
        {
            isAssembly = true;
        }
        else if (task == "2")
        {
            isAssembly = false;
        }
        else if (task == "3")
        {
            isSwap = true;
        }

         

        if (!isAssembly)
        {
            if (isSwap)
            {
                taskText.text = "TASK :SWAP";
                InputName.text = "Swap_";
                wheel2.gameObject.SetActive(false);
                isRotate = false;
                this.transform.position = _CoreWheel2.GetComponent<CoreWheel>()._Wheel.transform.position;
                this.transform.rotation = _CoreWheel2.GetComponent<CoreWheel>()._Wheel.transform.rotation;
            }
            else
            {
                taskText.text = "TASK :DEASSEMBLY";
                InputName.text = "Deassembly_";
                this.transform.position = _CoreWheel.GetComponent<CoreWheel>()._Wheel.transform.position;
                this.transform.rotation = _CoreWheel.GetComponent<CoreWheel>()._Wheel.transform.rotation;
            }

        }
        else
        {
            taskText.text = "TASK :ASSEMBLY";
            InputName.text = "Assembly_";
            
        }

        dataList = new List<MouseData>();
        InvokeRepeating("getMousePosition", 0.0f, 0.5f);
    }

    void getMousePosition()
    {
        long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        if (_isSelected)
        {
            MouseData data = new MouseData();
            data.logicalTime = logicaaTime;
            data.mTime = milliseconds;
            data.positionX = transform.position.x;
            data.positionY = transform.position.y;
            data.positionZ = transform.position.z;
            logicaaTime += 0.5f;
            dataList.Add(data);

            Debug.Log(transform.position);
            Debug.Log(dataList.Count);
        }
    }


    void Update()
    {

        

        if (isSave)
        {
            InputName.gameObject.SetActive(false);
            btnSave.gameObject.SetActive(false);
        }
        else
        {
            InputName.gameObject.SetActive(isComplete);
            btnSave.gameObject.SetActive(isComplete);
        }

        if (isComplete)
        {
            statusText.text = "Complete";
            statusText.color = Color.green;
        }
        else
        {
            statusText.text = "Incomplete";
            statusText.color = Color.red;

        }



        if (_isTriggerCore&&!_isSelected)
        {
            if (isAssembly)
            {
                isComplete = true;
            }
            else if (isSwap)
            {
                isComplete = true;
            }else if (!isAssembly)
            {
                isComplete = false;
            }
            this.transform.position = _CoreWheel.GetComponent<CoreWheel>()._Wheel.transform.position;
            this.transform.rotation = _CoreWheel.GetComponent<CoreWheel>()._Wheel.transform.rotation;
            return;

        }

        if (isRotate)
        {
            transform.Rotate(new Vector3(180, 0, 0) * Time.deltaTime);
        }

        if (!_isSelected)
        {
            return;
        }

        if (_isSelected)
        {
            //tracking mouse position.
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x * sensitive, Input.mousePosition.y * sensitive, positionOfScreen.z * sensitive);

            //converting screen position to world position with offset changes.
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offsetValue;

            if (currentPosition.y < 1)
            {
                currentPosition.y = 1;
            }

            transform.position = currentPosition;
        }

    }

    public bool _isSelected;
    public void OnMouseDown()
    {
        if (!_isSelected)
        {

            _isSelected = true;
            this.transform.localScale = _OrignalScale + Vector3.one + Vector3.one;

            if (isRotate)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, originalRotationValue, Time.time * rotationResetSpeed);
                isRotate = false;
            }


            isMouseDragging = !isMouseDragging;
            //Converting world position to screen position.
            positionOfScreen = Camera.main.WorldToScreenPoint(getTarget.transform.position);
            offsetValue = getTarget.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x * sensitive, Input.mousePosition.y * sensitive, positionOfScreen.z * sensitive));

        }
        else
        {

            if (isAssembly || isSwap)
            {
                isComplete = false;
            }
            else
            {
                isComplete = true;
            }

            _isSelected = false;
            isMouseDragging = false;
            this.transform.localScale = _OrignalScale;
            if (!_isTriggerCore)
            {
                isRotate = true;
            }
        }
    }

    public void OnMouseUp()
    {
        /*_isSelected=false;
        isMouseDragging = false;
        this.transform.localScale = _OrignalScale;
        if (!_isTriggerCore)
        {
            isRotate= true;
        }*/
    }

    public int _WheelId;
    public bool _isTriggerCore;
    public GameObject _CoreWheel;
    public GameObject _CoreWheel2;

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "CoreWheel")
        {
            if (_WheelId == other.GetComponent<CoreWheel>()._WheelId)
            {
                    
                _CoreWheel = other.gameObject;
                _isTriggerCore = true;
                Debug.Log(_isTriggerCore);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "CoreWheel")
        {
            _isTriggerCore= false;
        }
    }



}
