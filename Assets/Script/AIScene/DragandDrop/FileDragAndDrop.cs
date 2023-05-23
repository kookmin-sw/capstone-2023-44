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
        UnityDragAndDropHook.OnDroppedFiles += display_FilesAddress; // Drag Drop�� �۵��ϴ� �κ�
        // must be installed on the main thread to get the right thread id.

    }
    void OnDisable()
    {
        UnityDragAndDropHook.OnDroppedFiles -= display_FilesAddress; // ��ο� ���� ����ó���� ������ �߰��� event�������� dll�� UnInstall�ϴ� �κ�
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
        //���⼭ ���ʹ� �о�� file��θ� ������ �����ϴ� �κ�
        var fi = new System.IO.FileInfo(aFiles[0]);
        var ext = fi.Extension.ToLower();
        string imgFile = "";
        if (ext == ".png" || ext == ".jpg" || ext == ".jpeg") // �̹��� �����϶��� �о������
        {
            imgFile = aFiles[0];
        }
        if(imgFile != "") // �̹��������϶���
        {
            Debug.LogError(imgFile);
            string dest_file = "C:/Users/myPC/Novel/Novel_Data/InputImage/input0" + ext; // Build�� ���ϰ�ο� ����
            //File.Move(imgFile, dest_file);
            System.IO.File.Copy(imgFile, dest_file, true);
            gameObject.SetActive(false);
        }
        else //Image������ �ƴҋ��� �ٽ� �̹��� ������ �о������ �ϴ� ��Ʈ
        {
           
        }
    }

}
