#include "IUnityGraphics.h"

//make sure this appears before IUnityGraphicsD3D11
#include "d3d11.h"

#include "IUnityGraphicsD3D11.h"


// debug event
typedef void (*FuncPtr)(const char *);
FuncPtr Debug;

namespace globals {
ID3D11Device *device = nullptr;
ID3D11DeviceContext *context = nullptr;
} // namespace globals

extern "C" {

UNITY_INTERFACE_EXPORT void SetDebugFunction(FuncPtr fp) { Debug = fp; }

// Plugin function to handle a specific rendering event
static void UNITY_INTERFACE_API  UNITY_INTERFACE_API OnRenderEvent(int eventID) {
	Debug("Hello world");
  }


// Unity plugin load event
void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
UnityPluginLoad(IUnityInterfaces *unityInterfaces) {
  auto s_UnityInterfaces = unityInterfaces;
  IUnityGraphicsD3D11 *d3d11 = unityInterfaces->Get<IUnityGraphicsD3D11>();
  globals::device = d3d11->GetDevice();
  globals::device->GetImmediateContext(&globals::context);
}

// Unity plugin unload event
void UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API UnityPluginUnload() {}

// Freely defined function to pass a callback to plugin-specific scripts
UnityRenderingEvent UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API
getEventFunction() {
  return OnRenderEvent;
}
}


