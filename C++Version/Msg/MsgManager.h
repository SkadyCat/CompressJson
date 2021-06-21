

#pragma once
#include"MsgEntity.h"
#include <queue>
#include <string>

using namespace std;
class MsgManager
{
public:
	int index;
	MsgManager();
	~MsgManager();
	string preProcess;
	map<string, SerializeNode*> headMap;
	map<string, SerializeNode*> serialsNodes;
	
	queue<string> load(string path);
	void process(string path);
	void tranverse(string msg);
	SerializeNode* serialize(string msg, string jdata, SerializeNode*& outSerials, SerializeNode*& tail );
	queue<DynamicNode> serialize(string msg, string jdata);
	Layer serializeToLayer(string msg, string jdata);
	string deSerialize(Layer& layer);
	string deSerialize(SerializeNode*& head);
	SerializeNode* deSerialize(Layer layer,map<string,int>& posMaps,SerializeNode*& info,SerializeNode*& growNode);
	
	vector<string> Buffer2Json(string buf);
	string Json2Buffer(string msgType,string json);

	Layer fromBuffer(string buf) {
		
		Layer lb;
		lb.fromBuffer(buf);

		return lb;
	}
	Layer genLayer(string to) {
		Layer lb;
		lb.init(to);
		return lb;
	}
private:

};
