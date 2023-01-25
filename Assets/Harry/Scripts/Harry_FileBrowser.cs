using UnityEngine;
using System.Collections;
using System.IO;
using SimpleFileBrowser;
using UnityEngine.Video;
using Photon.Pun;

public class Harry_FileBrowser : MonoBehaviourPun
{
    VideoPlayer vp;
    Vector3 scale;

    // Warning: paths returned by FileBrowser dialogs do not contain a trailing '\' character
    // Warning: FileBrowser can only show 1 dialog at a time

    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        scale = transform.localScale / transform.parent.localScale.x;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 위치에 레이
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("SlotItem")) && hit.collider.gameObject == gameObject)
            {
                // Set filters (optional)
                // It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
                // if all the dialogs will be using the same filters
                FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"), new FileBrowser.Filter("Videos", ".mp4", ".mpg", ".avi"));

                // Set default filter that is selected when the dialog is shown (optional)
                // Returns true if the default filter is set successfully
                // In this case, set Images filter as the default filter
                FileBrowser.SetDefaultFilter(".mp4");

                // Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
                // Note that when you use this function, .lnk and .tmp extensions will no longer be
                // excluded unless you explicitly add them as parameters to the function
                FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

                // Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
                // It is sufficient to add a quick link just once
                // Name: Users
                // Path: C:\Users
                // Icon: default (folder icon)
                FileBrowser.AddQuickLink("Users", "C:\\Users", null);

                // Show a save file dialog 
                // onSuccess event: not registered (which means this dialog is pretty useless)
                // onCancel event: not registered
                // Save file/folder: file, Allow multiple selection: false
                // Initial path: "C:\", Initial filename: "Screenshot.png"
                // Title: "Save As", Submit button text: "Save"
                // FileBrowser.ShowSaveDialog( null, null, FileBrowser.PickMode.Files, false, "C:\\", "Screenshot.png", "Save As", "Save" );

                // Show a select folder dialog 
                // onSuccess event: print the selected folder's path
                // onCancel event: print "Canceled"
                // Load file/folder: folder, Allow multiple selection: false
                // Initial path: default (Documents), Initial filename: empty
                // Title: "Select Folder", Submit button text: "Select"
                // FileBrowser.ShowLoadDialog( ( paths ) => { Debug.Log( "Selected: " + paths[0] ); },
                //						   () => { Debug.Log( "Canceled" ); },
                //						   FileBrowser.PickMode.Folders, false, null, null, "Select Folder", "Select" );

                // Coroutine example
                StartCoroutine(ShowLoadDialogCoroutine());
            }
            else if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("SlotItem")) && hit.collider.transform == transform.GetChild(0))
            {
                //iTween.ScaleTo(gameObject, iTween.Hash("x", scale.x / 2, "y", scale.y / 2, "z", scale.z / 2, "time", 0.5f, "easetype", iTween.EaseType.easeInOutBack));
                photonView.RPC("RPCSetSize", RpcTarget.AllBuffered, 0.5f);
            }
            else if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("SlotItem")) && hit.collider.transform == transform.GetChild(1))
            {
                //iTween.ScaleTo(gameObject, iTween.Hash("x", scale.x, "y", scale.y, "z", scale.z, "time", 0.5f, "easetype", iTween.EaseType.easeInOutBack));
                photonView.RPC("RPCSetSize", RpcTarget.AllBuffered, 1f);
            }
            else if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("SlotItem")) && hit.collider.transform == transform.GetChild(2))
            {
                //iTween.ScaleTo(gameObject, iTween.Hash("x", scale.x * 1.5f, "y", scale.y * 1.5f, "z", scale.z * 1.5f, "time", 0.5f, "easetype", iTween.EaseType.easeInOutBack));
                photonView.RPC("RPCSetSize", RpcTarget.AllBuffered, 1.5f);
            }
        }

        if (!GameObject.Find("Canvas").transform.Find("Tree_Customize").gameObject.activeSelf)
        {
            foreach (Transform tr in transform)
            {
                tr.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (Transform tr in transform)
            {
                tr.gameObject.SetActive(true);
            }
        }
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: both, Allow multiple selection: true
        // Initial path: default (Documents), Initial filename: empty
        // Title: "Load File", Submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

        // Dialog is closed
        // Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            // Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log(FileBrowser.Result[i]);

            // Read the bytes of the first file via FileBrowserHelpers
            // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);

            //vp.source = VideoSource.Url;
            //vp.url = FileBrowser.Result[0];
            //vp.Play();
            photonView.RPC("RPCPlay", RpcTarget.AllBuffered, FileBrowser.Result[0]);

            //// Or, copy the first file to persistentDataPath
            //string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
            //FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
        }
    }

    [PunRPC]
    void RPCSetSize(float size)
    {
        iTween.ScaleTo(gameObject, iTween.Hash("x", scale.x * size, "y", scale.y * size, "z", scale.z * size, "time", 0.5f, "easetype", iTween.EaseType.easeInOutBack));
    }

    [PunRPC]
    void RPCPlay(string url)
    {
        vp.url = url;
        vp.Play();
    }
}