using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

using static TYPE;

public class XMLManager : MonoBehaviour
{
    public List<Thesis> LoadThesisXML()
    {
        //�����͸� ������ ���� ���� �� �����б�
        string filepath = Application.dataPath + "/Resources/Datas/thesis.xml";

        XmlDocument doc = new XmlDocument();

        List<Thesis> thesisDatas = new();

        doc.Load(filepath);

        //��Ʈ ����
        XmlElement nodes = doc["ThesisList"];

        //��Ʈ���� ��� ������
        foreach (XmlElement node in nodes.ChildNodes)
        {
            string academy = node.GetAttribute("academy");
            string author = node.GetAttribute("author");
            bool isValid = System.Convert.ToBoolean(node.GetAttribute("isValid"));

            Thesis readData = new Thesis(academy, author, isValid, THESIS);

            //������ �����͸� ����Ʈ�� �Է�
            thesisDatas.Add(readData);
        }

        return thesisDatas;

    }

    public List<Document> LoadDocuXML()
    {
        //�����͸� ������ ���� ���� �� �����б�
        string filepath = Application.dataPath + "/Resources/Datas/document.xml";

        XmlDocument doc = new XmlDocument();

        List<Document> docuDatas = new();

        doc.Load(filepath);

        //��Ʈ ����
        XmlElement nodes = doc["DocumentList"];

        //��Ʈ���� ��� ������
        foreach (XmlElement node in nodes.ChildNodes)
        {
            string institution = node.GetAttribute("institution");
            string stampFileName = node.GetAttribute("filename");
            bool isValid = System.Convert.ToBoolean(node.GetAttribute("isValid"));

            Document readData = new Document(institution, stampFileName, isValid, DOCUMENT);

            //������ �����͸� ����Ʈ�� �Է�
            docuDatas.Add(readData);
        }

        return docuDatas;

    }

}