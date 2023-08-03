//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.10
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------


using System;
using System.Runtime.InteropServices;

namespace Noesis
{

public class RequestNavigateEventArgs : RoutedEventArgs {
  private HandleRef swigCPtr;

  internal RequestNavigateEventArgs(IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn) {
    swigCPtr = new HandleRef(this, cPtr);
  }

  internal static HandleRef getCPtr(RequestNavigateEventArgs obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  ~RequestNavigateEventArgs() {
    Dispose();
  }

  public override void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          NoesisGUI_PINVOKE.delete_RequestNavigateEventArgs(swigCPtr);
        }
        swigCPtr = new HandleRef(null, IntPtr.Zero);
      }
      GC.SuppressFinalize(this);
      base.Dispose();
    }
  }

  internal static new void InvokeHandler(Delegate handler, IntPtr sender, IntPtr args) {
    RequestNavigateEventHandler handler_ = (RequestNavigateEventHandler)handler;
    if (handler_ != null) {
      handler_(Extend.GetProxy(sender, false), new RequestNavigateEventArgs(args, false));
    }
  }

  public string Uri {
    get {
      IntPtr strPtr = NoesisGUI_PINVOKE.RequestNavigateEventArgs_Uri_get(swigCPtr);
      string str = Noesis.Extend.StringFromNativeUtf8(strPtr);
      return str;
    }
  }

  public string Target {
    get {
      IntPtr strPtr = NoesisGUI_PINVOKE.RequestNavigateEventArgs_Target_get(swigCPtr);
      string str = Noesis.Extend.StringFromNativeUtf8(strPtr);
      return str;
    }
  }

  public RequestNavigateEventArgs(object source_, RoutedEvent event_, string uri_, string target_) : this(NoesisGUI_PINVOKE.new_RequestNavigateEventArgs(Noesis.Extend.GetInstanceHandle(source_), RoutedEvent.getCPtr(event_), uri_ != null ? uri_ : string.Empty, target_ != null ? target_ : string.Empty), true) {
  }

}

}

