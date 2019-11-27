// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"



#include "vdjPlugin8.h"

	HRESULT VDJ_API DllGetClassObject(const GUID & rclsid, const GUID & riid, void** ppObject)
	{
		if (memcmp(&rclsid, &CLSID_VdjPlugin8, sizeof(GUID)) == 0 && memcmp(&riid, &IID_IVdjPluginDsp8, sizeof(GUID)) == 0)
		{
			*ppObject = new CMyPlugin8();
		}
		else
		{
			return CLASS_E_CLASSNOTAVAILABLE;
		}

		return NO_ERROR;
	}


