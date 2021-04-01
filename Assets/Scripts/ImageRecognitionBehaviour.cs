using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class ImageRecognitionBehaviour : MonoBehaviour
{

    private ARTrackedImageManager arTrackedImageManager;

    [SerializeField]
    private GameObject[] objectsToPlace;

    private Dictionary<string, GameObject> imageObjects = new Dictionary<string, GameObject>();

    private void Awake()
    {
        arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach (var placeObject in objectsToPlace)
        {
            GameObject newObject = Instantiate(placeObject, Vector3.zero, Quaternion.identity);
            newObject.name = placeObject.name;
            newObject.SetActive(false);
            imageObjects.Add(newObject.name, newObject);
        }
    }

    public void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnImageChange;
    }

    public void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChange;
    }


    public void OnImageChange(ARTrackedImagesChangedEventArgs args)
    {
        //What to do for images newly found
        foreach (var trackedImage in args.added)
        {
            //make sure we've got images to track
            if (imageObjects != null)
            {
                Debug.Log("DEBUG: Image: " + trackedImage.name);
                imageObjects[trackedImage.referenceImage.name].SetActive(true);
                imageObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
                //imageObjects[trackedImage.name].transform.rotation = trackedImage.transform.rotation;
            }
        }

        //what to do for images that have been updated (position)
        foreach (var trackedImage in args.updated)
        {
            //make sure we've got images to track
            if (imageObjects != null)
            {
                imageObjects[trackedImage.referenceImage.name].SetActive(true);
                imageObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
            }
        }

        //what to do when image is lost/can't be found
        foreach (var trackedImage in args.removed)
        {
            //do nothing for now, let object exist where it was last detected
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}