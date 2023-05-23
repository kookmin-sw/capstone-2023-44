using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thesis : Paper
{

    public string author { get; set; }

    public string academy { get; set; }

    public Thesis(string author, string academy, bool isValid, TYPE type)
    {
        this.academy = academy;
        this.author = author;
        this.isValidPaper = isValid;
        this.paperType = type;
    }

}
