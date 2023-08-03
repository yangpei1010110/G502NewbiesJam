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

public partial class PropertyMetadata : BaseComponent {
  internal new static PropertyMetadata CreateProxy(IntPtr cPtr, bool cMemoryOwn) {
    return new PropertyMetadata(cPtr, cMemoryOwn);
  }

  internal PropertyMetadata(IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn) {
  }

  internal static HandleRef getCPtr(PropertyMetadata obj) {
    return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
  }

  public object DefaultValue {
    get {
      IntPtr cPtr = GetDefaultValueHelper();
      return Noesis.Extend.GetProxy(cPtr, true);
    }
    set {
      object def = value;
      if (def is Type) {
        def = Noesis.Extend.GetResourceKeyType((Type)def);
      }
      SetDefaultValueHelper(def);
    }
  }

  public bool HasDefaultValue {
    get {
      bool ret = NoesisGUI_PINVOKE.PropertyMetadata_HasDefaultValue_get(swigCPtr);
      return ret;
    } 
  }

  new internal static IntPtr GetStaticType() {
    IntPtr ret = NoesisGUI_PINVOKE.PropertyMetadata_GetStaticType();
    return ret;
  }

  private IntPtr GetDefaultValueHelper() {
    IntPtr ret = NoesisGUI_PINVOKE.PropertyMetadata_GetDefaultValueHelper(swigCPtr);
    return ret;
  }

  private void SetDefaultValueHelper(object value) {
    NoesisGUI_PINVOKE.PropertyMetadata_SetDefaultValueHelper(swigCPtr, Noesis.Extend.GetInstanceHandle(value));
  }

}

}

