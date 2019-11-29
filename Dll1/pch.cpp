#include "pch.h"
#include <string>


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



	return S_OK;
}
//-----------------------------------------------------------------------------
HRESULT VDJ_API CMyPlugin8::OnGetPluginInfo(TVdjPluginInfo8* infos)
{
	infos->PluginName = "MyPlugin8";
	infos->Author = "Atomix Productions";
	infos->Description = "My first VirtualDJ 8 plugin";
	infos->Version = "1.0";
	infos->Flags = 0x00;
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

HRESULT VDJ_API CMyPlugin8::OnProcessSamples(float* buffer, int nb)
{
	std::string result = "";
	result = AddToString("fileName", GetStringFromVDJ("get loaded_song \"Filename\""), result);
	result = AddToString("filePath", GetStringFromVDJ("get loaded_song \"Filepath\""), result);
	result = AddToString("beatNum", GetStringFromVDJ("get_beat_num"), result);

	result = AddToString("beatBar16", GetStringFromVDJ("get Beat_bar 16"), result);
	result = AddToString("beatBar", GetStringFromVDJ("get Beat_bar"), result);
	result = AddToString("beatPos", GetStringFromVDJ("get_beatpos"), result);
	result = AddToString("bpm", GetStringFromVDJ("get_bpm"), result);
	result = AddToString("position", GetStringFromVDJ("get_position"), result);

	result = AddToString("volume", GetStringFromVDJ("get_volume"), result);
	result = AddToString("deck", GetStringFromVDJ("get deck"), result);
	result = AddToString("crossfader", GetStringFromVDJ("crossfader"), result);
	result = AddToString("elapsed", GetStringFromVDJ("get_time elapsed 1000"), result);
	result = result.append("\r\n");



	if (result.length() > 0)
	{

		DWORD numWritten;
		if (pipe !=0 && pipe != NULL && !WriteFile(pipe, result.c_str(), result.length(), &numWritten, NULL))
		{
			this->OnStart();
		};

	}
	return S_OK;
}

std::string CMyPlugin8::AddToString(const char* vdjCommand, std::string result, std::string source)
{
	source.append(vdjCommand);
	source.append("=");
	source.append(result);
	source.append(",");
	return source;
}

HRESULT VDJ_API CMyPlugin8::OnStart()
{
	pipe = CreateFile(TEXT("\\\\.\\pipe\\virtualDJ"), GENERIC_READ | GENERIC_WRITE, 0, NULL, OPEN_EXISTING, 0, NULL);
	bool connected = ConnectNamedPipe(pipe, NULL);
	if (!connected)
	{
		//DisconnectNamedPipe(pipe);
	//	pipe = NULL;
	}
	return 0;
}

HRESULT VDJ_API CMyPlugin8::OnStop()
{
	if (pipe != 0)
	{
		DisconnectNamedPipe(pipe);
		pipe = NULL;
	}
	return 0;
}

std::string CMyPlugin8::GetStringFromVDJ(const char* vdjCommand)
{
	HRESULT hr;
	std::string fileName = "";
	char temp_filepath[512];
	hr = GetStringInfo(vdjCommand, temp_filepath, 512);
	fileName.append(temp_filepath);
	fileName.erase(fileName.find_last_not_of(" \n\r\t") + 1);
	return fileName;
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