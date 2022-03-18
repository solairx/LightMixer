#include "pch.h"
#include <string>
#include <thread> 
#include <sstream>
#include <iostream>
#include <fstream> // add this

//-----------------------------------------------------------------------------
HRESULT VDJ_API CMyPlugin8::OnLoad()
{
	// ADD YOUR CODE HERE WHEN THE PLUGIN IS CALLED
	m_Reset = 0;
	m_Dry = 0.0f;
	m_Wet = 0.0f;

	bool bMasterFX = isMasterFX();


	DeclareParameterButton(&m_Reset, ID_BUTTON_1, "Reset", "R");
	DeclareParameterSlider(&m_Wet, ID_SLIDER_1, "Dry/Wet", "D/W", 1.0f);

	OnParameter(ID_SLIDER_1);

	//Sleep(5000);
	//this->OnStart();
	return S_OK;
}
//-----------------------------------------------------------------------------
HRESULT VDJ_API CMyPlugin8::OnGetPluginInfo(TVdjPluginInfo8* infos)
{
	infos->PluginName = "SolairxLight";
	infos->Author = "SolairX Productions";
	infos->Description = "Light Automation";
	infos->Version = "1.1";
	infos->Flags = VDJFLAG_PROCESSFIRST;
	infos->Bitmap = NULL;
	return S_OK;
}
//---------------------------------------------------------------------------
ULONG VDJ_API CMyPlugin8::Release()
{
	// ADD YOUR CODE HERE WHEN THE PLUGIN IS RELEASED

	delete this;
	return 0;
}
//---------------------------------------------------------------------------
HRESULT VDJ_API CMyPlugin8::OnGetUserInterface(TVdjPluginInterface8* pluginInterface)
{
	pluginInterface->Type = VDJINTERFACE_DEFAULT;

	return S_OK;
}
//---------------------------------------------------------------------------
HRESULT VDJ_API CMyPlugin8::OnParameter(int id)
{
	switch (id)
	{
	case ID_BUTTON_1:
		if (m_Reset == 1)
		{
			m_Wet = 0.5f;
			HRESULT hr;
			hr = SendCommand("effect_slider 1 50%");

		}
		break;

	case ID_SLIDER_1:
		wchar_t str_author[512] = TEXT("");
		double qRes;

		m_Dry = 1 - m_Wet;
		break;
	}

	return S_OK;
}
HANDLE pipe;
int test;

HRESULT VDJ_API CMyPlugin8::OnProcessSamples(float* buffer, int nb)
{
	return S_OK;
}


bool thrdStart = false;
void CMyPlugin8::task1()
{
	//pipe = CreateFile(TEXT("\\\\.\\pipe\\virtualDJ"), GENERIC_READ | GENERIC_WRITE, 0, NULL, OPEN_EXISTING, 0, NULL);
//	bool connected = ConnectNamedPipe(pipe, NULL);
//	if (!connected)
	{
		//DisconnectNamedPipe(pipe);
	//	pipe = NULL;
	}
	
	while (thrdStart)
	{
	//	std::stringstream ss;

		DWORD numWritten;
		std::ofstream deck1{ TEXT("\\\\.\\pipe\\virtualDJ"), std::wofstream::trunc };
		std::string crossfader = GetStringFromVDJ("crossfader");
				
		deck1 << "fileName=" <<  GetStringFromVDJ("deck  1 get loaded_song \"Filename\"") << ",";
		deck1 << "filePath=" << GetStringFromVDJ("deck  1 get loaded_song \"Filepath\" ") << ",";
		deck1 << "beatPos=" << GetStringFromVDJ("deck  1 get_beatpos") << ",";
		deck1 << "bpm=" << GetStringFromVDJ("deck  1 get_bpm") << ",";
		deck1 << "position=" << GetStringFromVDJ("deck  1 get_position") << ",";
		deck1 << "volume=" << GetStringFromVDJ("deck  1 get_volume") << ",";
		deck1 << "deck=1,";
		deck1 << "crossfader=" << crossfader << ",";
		deck1 << "elapsed=" << GetStringFromVDJ("deck  1 get_time elapsed 1000") << ",";
		deck1 << "\r\n";
		Sleep(500);
		std::ofstream deck2{ TEXT("\\\\.\\pipe\\virtualDJ"), std::wofstream::trunc };
		deck2 << "fileName=" << GetStringFromVDJ("deck  2 get loaded_song \"Filename\"") << ",";
		deck2 << "filePath=" << GetStringFromVDJ("deck  2 get loaded_song \"Filepath\" ") << ",";
		deck2 << "beatPos=" << GetStringFromVDJ("deck  2 get_beatpos") << ",";
		deck2 << "bpm=" << GetStringFromVDJ("deck  2 get_bpm") << ",";
		deck2 << "position=" << GetStringFromVDJ("deck  2 get_position") << ",";
		deck2 << "volume=" << GetStringFromVDJ("deck  2 get_volume") << ",";
		deck2 << "deck=2,";
		deck2 << "crossfader=" << crossfader << ",";
		deck2 << "elapsed=" << GetStringFromVDJ("deck  2 get_time elapsed 1000") << ",";
		deck2 << "\r\n";
		
		Sleep(500);
	}
}
std::thread thrd;

HRESULT VDJ_API CMyPlugin8::OnStart()
{
	if (!thrdStart)
	{
		thrd = std::thread(&CMyPlugin8::task1, this);
	}
	thrdStart = true;
	return 0;
}



std::string CMyPlugin8::GetStringFromVDJ(const char* vdjCommand)
{
	std::string fileName = "";
	char temp_filepath[512];
    GetStringInfo(vdjCommand, temp_filepath, 512);
	
	fileName.append(temp_filepath);
	fileName.erase(fileName.find_last_not_of(" \n\r\t") + 1);
	
	return fileName;
}


HRESULT VDJ_API CMyPlugin8::OnStop()
{
	if (pipe != 0)
	{
		DisconnectNamedPipe(pipe);
		pipe = NULL;
	}
	if (thrdStart)
	{
		thrdStart = false;
		thrd.join();
	}
	
	
	return 0;
}



//---------------------------------------------------------------------------
HRESULT VDJ_API CMyPlugin8::OnGetParameterString(int id, char* outParam, int outParamSize)
{
	switch (id)
	{
	case ID_SLIDER_1:
		//sprintf(outParam, "+%.0f%%", m_Wet * 100);
		break;
	}

	return S_OK;
}

//-------------------------------------------------------------------------------------------------------------------------------------
// BELOW, ADDITIONAL FUNCTIONS ONLY TO EXPLAIN SOME FEATURES (CAN BE REMOVED)
//-------------------------------------------------------------------------------------------------------------------------------------
bool CMyPlugin8::isMasterFX()
{
	double qRes;
	HRESULT hr = S_FALSE;

	hr = GetInfo("get_deck 'master' ? true : false", &qRes);
	hr = GetInfo("get loaded_song 'author'", &qRes);

	if (qRes == 1.0f) return true;
	else return false;
}