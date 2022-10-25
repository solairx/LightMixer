// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.

#ifndef PCH_H
#define PCH_H

// add headers that you want to pre-compile here
#include "framework.h"

#endif //PCH_H

#define MYPLUGIN8_H

// we include stdio.h only to use the sprintf() function
// we define _CRT_SECURE_NO_WARNINGS for the warnings of the sprintf() function
#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>

#include "vdjPlugin8.h"
#include "vdjDsp8.h"
#include "string"

class CMyPlugin8 : public IVdjPlugin8
{
public:
	HRESULT VDJ_API OnLoad();
	HRESULT VDJ_API OnGetPluginInfo(TVdjPluginInfo8* infos);
	ULONG VDJ_API Release();
	HRESULT VDJ_API OnGetUserInterface(TVdjPluginInterface8* pluginInterface);
	HRESULT VDJ_API OnParameter(int id);
	HRESULT VDJ_API OnGetParameterString(int id, char* outParam, int outParamSize);
	HRESULT VDJ_API OnProcessSamples(float* buffer, int nb);
	std::string GetStringFromVDJ(const char* command);
	HRESULT VDJ_API OnStart();
	HRESULT VDJ_API OnStop();
	void task1();
private:
	int m_Reset;
	float m_Dry;
	float m_Wet;

	bool isMasterFX(); // an example of additional function for the use of GetInfo()

protected:
	typedef enum _ID_Interface
	{
		ID_BUTTON_1,
		ID_SLIDER_1
	} ID_Interface;
};
