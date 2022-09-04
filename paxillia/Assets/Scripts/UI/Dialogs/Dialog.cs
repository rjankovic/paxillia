using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Message
{ 
    public string Text { get; set; }
}

public class Dialog
{
    public List<Message> Messages { get; set; }
}
