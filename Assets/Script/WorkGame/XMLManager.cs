using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

using static TYPE;

public class XMLManager : MonoBehaviour
{
    public List<Thesis> LoadThesisXML()
    {
        //데이터를 형성할 문서 생성 및 파일읽기
        string filepath = Application.dataPath + "/Resources/Datas/thesis.xml";

        XmlDocument doc = new XmlDocument();

        List<Thesis> thesisDatas = new();

        doc.Load(filepath);

        //루트 설정
        XmlElement nodes = doc["ThesisList"];

        //루트에서 요소 꺼내기
        foreach (XmlElement node in nodes.ChildNodes)
        {
            string academy = node.GetAttribute("academy");
            string author = node.GetAttribute("author");
            bool isValid = System.Convert.ToBoolean(node.GetAttribute("isValid"));

            Thesis readData = new Thesis(academy, author, isValid, THESIS);

            //가져온 데이터를 리스트에 입력
            thesisDatas.Add(readData);
        }

        return thesisDatas;

    }

    public List<Document> LoadDocuXML()
    {
        //데이터를 형성할 문서 생성 및 파일읽기
        string filepath = Application.dataPath + "/Resources/Datas/document.xml";

        XmlDocument doc = new XmlDocument();

        List<Document> docuDatas = new();

        doc.Load(filepath);

        //루트 설정
        XmlElement nodes = doc["DocumentList"];

        //루트에서 요소 꺼내기
        foreach (XmlElement node in nodes.ChildNodes)
        {
            string institution = node.GetAttribute("institution");
            string stampFileName = node.GetAttribute("filename");
            bool isValid = System.Convert.ToBoolean(node.GetAttribute("isValid"));

            Document readData = new Document(institution, stampFileName, isValid, DOCUMENT);

            //가져온 데이터를 리스트에 입력
            docuDatas.Add(readData);
        }

        return docuDatas;

    }

}