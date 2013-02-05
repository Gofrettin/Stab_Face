using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

public class TextBoxTraceListener : TraceListener
{
    private static TextBoxTraceListener instance = null;
    private RichTextBox _target;

    private StringSendDelegate _invokeWrite;
    private TextBoxTraceListener(RichTextBox target)
    {
        _target = target;
        _invokeWrite = new StringSendDelegate(SendString);
    }

    public static TextBoxTraceListener getTraceListener(RichTextBox target)
    {
        if (instance == null)
        {
            instance = new TextBoxTraceListener(target);
        }
        return instance;
    }

    public override void Write(string message)
    {
        try
        {
            _target.Invoke(_invokeWrite, new object[] { message });

        }
        catch (Exception ex)
        {
        }
    }

    public override void WriteLine(string message)
    {
        try
        {
            _target.Invoke(_invokeWrite, new object[] { message + Environment.NewLine });

        }
        catch (Exception ex)
        {
        }
    }

    private delegate void StringSendDelegate(string message);
    private void SendString(string message)
    {
        // No need to lock text box as this function will only 
        // ever be executed from the UI thread
        _target.AppendText(message);
        _target.ScrollToCaret();
    }
}