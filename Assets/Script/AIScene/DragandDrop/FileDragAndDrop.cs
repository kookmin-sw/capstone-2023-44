using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using B83.Win32;
using System.IO;

public class FileDragAndDrop : MonoBehaviour
{
    public VGGScript Image_Detection;
    List<string> log = new List<string>();
    void OnEnable()
    {
        UnityDragAndDropHook.InstallHook();
        UnityDragAndDropHook.OnDroppedFiles += display_FilesAddress; // Drag Drop시 작동하는 부분
        // must be installed on the main thread to get the right thread id.

    }
    void OnDisable()
    {
        UnityDragAndDropHook.OnDroppedFiles -= display_FilesAddress; // 경로에 의한 파일처리가 끝난후 추가한 event를끝내고 dll을 UnInstall하는 부분
        UnityDragAndDropHook.UninstallHook();
        
    }

    public void display_FilesAddress(List<string> aFiles, POINT aPos)
    {

        // do something with the dropped file names. aPos will contain the 
        // mouse position within the window where the files has been dropped.
        //string str = "Dropped " + aFiles.Count + " files at: " + aPos + "\n\t" +
            //aFiles.Aggregate((a, b) => a + "\n\t" + b);
        //string fileLocation = aFiles.Count;
        Debug.LogError(aFiles[0]);
        //여기서 부터는 읽어온 file경로를 가지고 가공하는 부분
        var fi = new System.IO.FileInfo(aFiles[0]);
        var ext = fi.Extension.ToLower();
        string imgFile = "";
        if (ext == ".png" || ext == ".jpg" || ext == ".jpeg") // 이미지 파일일때만 읽어오도록
        {
            imgFile = aFiles[0];
        }
        if(imgFile != "") // 이미지파일일때만
        {
            Debug.LogError(imgFile);
            string dest_file = "C:/Users/myPC/Novel/Novel_Data/InputImage/input0" + ext; // Build된 파일경로에 저장
            //File.Move(imgFile, dest_file);
            System.IO.File.Copy(imgFile, dest_file, true);
            gameObject.SetActive(false);
        }
        else //Image파일이 아닐떄는 다시 이미지 파일을 읽어오도록 하는 파트
        {
           
        }
    }

}
