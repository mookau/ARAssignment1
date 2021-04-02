using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class ImageRecognitionBehaviour : MonoBehaviour
{

    private ARTrackedImageManager arTrackedImageManager;
    private ARSessionOrigin aRSessionOrigin;

    [SerializeField]
    private GameObject[] arCreaturesToPlace;

    private Dictionary<string, GameObject> imageObjects = new Dictionary<string, GameObject>();

    private void Awake()
    {
        arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        aRSessionOrigin = FindObjectOfType<ARSessionOrigin>();

        foreach (var creature in arCreaturesToPlace)
        {
            if (creature != null)
            {
                //Debug.Log("//Debug: Found: " + creature.name);
                var animalHandler = creature.GetComponent<AnimalHandler>();
                if (animalHandler != null)
                {
                    GameObject newObject = Instantiate(creature);
                    newObject.name = animalHandler.animalData.name;
                    newObject.SetActive(false);
                    imageObjects.Add(animalHandler.animalData.imageName, newObject);

                }

            }
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
                if (imageObjects[trackedImage.referenceImage.name] != null)
                {
                    //move found image to image position and rotation
                    imageObjects[trackedImage.referenceImage.name].SetActive(true);
                    imageObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
                    imageObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
                }
            }

            /*
            foreach (var creature in arCreaturesToPlace)
            {
                if (creature != null)
                {
                    //Debug.Log("//Debug: Found: " + creature.name);
                    var animalHandler = creature.GetComponent<AnimalHandler>();
                    if (animalHandler != null)
                    {
                        if (animalHandler.animalData.imageName == trackedImage.referenceImage.name)
                        {
                            GameObject newObject = Instantiate(creature, trackedImage.transform.position, trackedImage.transform.rotation);
                            newObject.name = animalHandler.animalData.name;
                            newObject.SetActive(true);
                            imageObjects.Add(animalHandler.animalData.imageName, newObject);
                        }
                    }
                    else if(Debug.isDebugBuild)
                    {
                        Debug.Log("//Debug: Object doesn't have Animal Handler Component");
                    }
                    
                }
            }
            */
        }

        //what to do for images that have been updated (position)
        foreach (var trackedImage in args.updated)
        {
            //make sure we've got images to track
            if (imageObjects != null)
            {
                if (imageObjects[trackedImage.referenceImage.name] != null)
                {
                    //move found image to image position and rotation
                    imageObjects[trackedImage.referenceImage.name].SetActive(true);
                    imageObjects[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
                    imageObjects[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;
                }
            }
        }

        //what to do when image is lost/can't be found
        foreach (var trackedImage in args.removed)
        {
            //do nothing for now, let object exist where it was last detected
            //if we care about what is and isn't being actively tracked, we could disable lost images
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