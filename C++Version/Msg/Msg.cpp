#include <unordered_map>
#include <fstream>
#include <iostream>
#include <string>
#include <map>
#include <thread>
#include <mutex>
#include "../Msg/MsgEntity.h"
#include "../Msg/MsgManager.h"
using namespace std;

mutex m;

extern "C"
{
	#include "../Lua/src/lua.h"  
	#include "../Lua/src/lualib.h"  
	#include "../Lua/src/lauxlib.h"  
}

//unordered_map<string,>

struct SerialsNode {
public:
	string type;
	int index;
	void* data;
	SerialsNode* next;
};

void deserialize(int* IArr,float* FArr,string* SArr) {
	
	

}
static int _new(lua_State* L){
	char data[1000];
	MsgManager * manager = new MsgManager();
	lua_pushlightuserdata(L,manager);
	return 1;
}
static int _load(lua_State* L){
	MsgManager* manager = (MsgManager*)lua_touserdata(L, 1);
	string path = luaL_checkstring(L, 2);
	manager->process(path);
	return 0;
}
static int _Json2Buffer(lua_State*L){
	MsgManager* manager = (MsgManager*)lua_touserdata(L, 1);
	m.lock();
	string type = luaL_checkstring(L,2);
	string json = luaL_checkstring(L,3);
	string val = manager->Json2Buffer(type,json);

	m.unlock();
	lua_pushlstring(L,val.c_str(),val.size());
	return 1;
}
static int _Buffer2Json(lua_State*L){
	MsgManager* manager = (MsgManager*)lua_touserdata(L, 1);
	m.lock();
	size_t s;
	const char* buffer = luaL_checklstring(L,2,&s);
	string data;
	for(int i =0;i<s;i++){
		data.push_back(buffer[i]);
	}
	vector<string> val = manager->Buffer2Json(data);
	m.unlock();
	lua_pushlstring(L,val[0].c_str(),val[0].size());
	lua_pushlstring(L,val[1].c_str(),val[1].size());
	return 2;
}

static luaL_Reg luaLibs[] =
{
	{"load", _load},
	{"new", _new},
	{"Json2Buffer",_Json2Buffer},
	{"Buffer2Json",_Buffer2Json},
	{ NULL, NULL }
};


#if defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(__NT__)
extern "C"  __declspec(dllexport) int luaopen_Msg(lua_State * L)
{
	lua_newtable(L);
	luaL_setfuncs(L, luaLibs, 0);
	return 1;
}
#endif
#ifdef _WIN64
	
#endif

#if __linux__
extern "C"  int luaopen_Msg(lua_State * L)
{
	lua_newtable(L);
	luaL_setfuncs(L, luaLibs, 0);
	return 1;
}
#endif
