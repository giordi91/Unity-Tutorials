using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.Runtime.InteropServices;

public class nativePluginNET: MonoBehaviour {


    //the name of the DLL you want to load stuff from
    private const string pluginName = "nativePlugin";
    //native interface
    [DllImport(pluginName)]
    private static extern IntPtr getEventFunction();


    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void DebugDelegate(string str);

    static void CallBackFunction(string str) { Debug.Log(str); }

    [DllImport(pluginName)]
    public static extern void SetDebugFunction(IntPtr fp);

    private CommandBuffer cmd;

    // Use this for initialization
    void Start () {

        DebugDelegate callback_delegate = new DebugDelegate(CallBackFunction);
        // Convert callback_delegate into a function pointer that can be
        // used in unmanaged code.
        IntPtr intptr_delegate =
            Marshal.GetFunctionPointerForDelegate(callback_delegate);
        // Call the API passing along the function pointer.
        SetDebugFunction(intptr_delegate);


        //crating the command buffer and attaching it to camera
        cmd = new CommandBuffer();
	    cmd.name = pluginName; 
        var camera = Camera.main;
        camera.AddCommandBuffer(CameraEvent.AfterGBuffer, cmd);
    }

	// Update is called once per frame
	void Update () {

        //we jump into native to be able to generate the culling mimaps
        cmd.IssuePluginEvent(getEventFunction(),0);

	}

}
