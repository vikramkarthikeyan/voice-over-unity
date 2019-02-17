using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IBM.Watson.DeveloperCloud.Connection;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;
using IBM.Watson.DeveloperCloud.Utilities;


public class FaceDetectorScript : MonoBehaviour
{

    public Text dataOutput;
    public VisualRecognition _visualRecognition;
    private string _iamApikey = "vIDkMZGVPIBZpF8TpYVvlhuay2i15zXCDxcSNfjHCFUN";
    private string _serviceUrl = "https://gateway.watsonplatform.net/visual-recognition/api";
    
    // Start is called before the first frame update
    void Start()
    {

         //Credentials credentials = new Credentials("", "");
         //_visualRecognition = new VisualRecognition(credentials);
         //_visualRecognition.VersionDate = "2018-02-16";
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());
        
    }

    private IEnumerator CreateService()
    {
        if (string.IsNullOrEmpty(_iamApikey))
        {
            throw new WatsonException("Plesae provide IAM ApiKey for the service.");
        }

        //  Create credential and instantiate service
        Credentials credentials = null;

        //  Authenticate using iamApikey
        TokenOptions tokenOptions = new TokenOptions()
        {
            IamApiKey = _iamApikey
        };

        credentials = new Credentials(tokenOptions, _serviceUrl);

        //  Wait for tokendata
        while (!credentials.HasIamTokenData())
            yield return null;

        _visualRecognition = new VisualRecognition(credentials);
        _visualRecognition.VersionDate = "2018-02-16";
    }


    public void DetectFaces(string path)
    {
        Debug.Log(path);
        Debug.Log("Inside DetecFaces Method");
        //  Classify using image url
        if(!_visualRecognition.DetectFaces(path, OnDetectFaces, OnFail))
        {
            Log.Debug("ExampleVisualRecognition.DetectFaces()", "Detect faces failed!");
        }
        else
        {
            Debug.Log("Calling Watson");
            dataOutput.text = "";
        }
    }

    public void OnDetectFaces(DetectedFaces multipleImages, Dictionary<string, object> customData)
    {
        Debug.Log("Inside OnDetectFaces Method Successfully");
        var data = multipleImages.images[0].faces[0];
        dataOutput.text = data.age.max.ToString();
        Debug.Log(data.face_location);
        Log.Debug("ExampleVisualRecognition.OnDetectFaces()", "Detect faces result: {0}", customData["json"].ToString());
    }

    public void OnFail(RESTConnector.Error error, Dictionary<string, object> customData){
        Log.Debug("ExampleVisualRecognition.OnFail()", "Error Received", error.ToString());
    }

    public void CaptureImage(){
        Debug.Log("Inside Capture Image");
        Debug.Log(Application.persistentDataPath);
        ScreenCapture.CaptureScreenshot("snapshot.jpg");
        Debug.Log(Application.persistentDataPath + "/snapshot.jpg");
        //DetectFaces(Application.persistentDataPath + "/snapshot.jpg");
        //DetectFaces("https://davidwalsh.name/demo/face-recognition/Oceans-Eleven.jpg");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            CaptureImage();
        }
        
    }

    
}
