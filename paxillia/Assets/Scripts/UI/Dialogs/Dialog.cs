using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Message
{ 
    public string Text { get; set; }

    public string Character { get; set; }

    public int Duration { get; set; } = 10;
}

public class Dialog
{
    public List<Message> Messages { get; set; }
}
