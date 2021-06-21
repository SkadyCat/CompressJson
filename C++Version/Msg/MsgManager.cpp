#include "MsgManager.h"
#include <iostream>
#include <fstream>

#include <cctype>
#include <algorithm>
#include "json11.hpp"
#include <sstream>
#include <stack>
#include <assert.h>

using namespace json11;


using namespace std;
MsgManager::MsgManager()
{
	index = 0;
}

MsgManager::~MsgManager()
{
}
std::string ltrim(const std::string &s)
{
	size_t start = s.find_first_not_of(WHITESPACE);
	return (start == std::string::npos) ? "" : s.substr(start);
}

std::string rtrim(const std::string &s)
{
	size_t end = s.find_last_not_of(WHITESPACE);
	return (end == std::string::npos) ? "" : s.substr(0, end + 1);
}

std::string trim(const std::string &s) {
	return rtrim(ltrim(s));
}
bool contains(string origin, string key) {
	int idx = origin.find(key);//��a�в���b.
	if (idx == string::npos)//�����ڡ�
		return false;
	return true;
}
//
string remove(string origin, string key) {
	std::string t = origin;
	std::string s = key;

	std::string::size_type i = t.find(s);

	if (i != std::string::npos)
		t.erase(i, s.length());

	return t;
}
vector<string> split(string s, string delimiter) {
	size_t pos_start = 0, pos_end, delim_len = delimiter.length();
	string token;
	vector<string> res;

	while ((pos_end = s.find(delimiter, pos_start)) != string::npos) {
		token = s.substr(pos_start, pos_end - pos_start);
		pos_start = pos_end + delim_len;
		res.push_back(token);
	}

	res.push_back(s.substr(pos_start));
	return res;
}
queue<string> MsgManager::load(string path)
{
	ifstream ifs(path);
	string line;
	string process;
	bool flag = false;
	queue<string> q;
	if (ifs.is_open())
	{
		while (getline(ifs, line))
		{
			string tt = trim(line);
			q.push(tt);
		}
		ifs.close();
	}
	return q;
}
template <class Type>
Type stringToNum(const string& str) {
	istringstream iss(str);
	Type num;
	iss >> num;
	return num;
}
template<typename T> string toString(const T& t) {
	ostringstream oss;  //创建一个格式化输出流
	oss << t;             //把值传递如流中
	return oss.str();
}

void MsgManager::process(string path)
{

	bool isOpen = false;
	queue<string> qq = load(path);
	int line = 0;
	
	while (!qq.empty()) {
		stringstream ss;
		string k = qq.front();

		line++;
		qq.pop();
		if (k == "") {
			continue;
		}
		
		if (contains(k, "message")) {
			isOpen = true;
			string head = trim(k);
			vector<string> headItems = split(head, " ");
			headMap[headItems[1]] = new SerializeNode(headItems[0], headItems[1], nullptr);
			
			SerializeNode* cur = headMap[headItems[1]];
			cur->isDynamic = false;
			while (!qq.empty())
			{
				line++;
				string msg = qq.front();
				qq.pop();
				if (msg == "") {
					continue;
				}
				msg = trim(msg);
				vector<string> msgItem = split(msg, " ");
				if (msg == "end") {
					isOpen = false;
					cur = cur->grow(msgItem[0], msgItem[0], 0);
					break;
				}

				if (msgItem[0] == "repeated") {
					ss << line;
					if (msgItem.size() < 3) {
						string val = "error: please confirm the proto txt file in line " + ss.str()+"\n";
						assert(msgItem.size() == 3 );
						return;
					}
					
					cur = cur->grow(msgItem[1], msgItem[2], 0);
					cur->isRepeated = true;
				}
				else {
					cur = cur->grow(msgItem[0], msgItem[1], 0);
				}

				if (contains(msg, "message")) {
					
					string val = "error: please confirm the message have end syntax line: " + ss.str() + "\n";
					assert(!contains(msg, "message"));
				}
				cur->isDynamic = false;
			}

			
			cur->isDynamic = false;
		}
	}

}

void MsgManager::tranverse(string msg)
{
	SerializeNode* cur = headMap[msg];
	while (cur != nullptr)
	{
		cur = cur->next;
	}
}


void* parseData(string type, Json val) {
	
	
	int num; // a variable of int data type
	float fnum;
	void* data = nullptr;
	if (type == "int") {
		data = new int(val.int_value());
	}
	if (type == "float") {
		data = new float(val.number_value());
	}
	if (type == "string") {
		data = new string(val.string_value());
	}
	if (type == "long") {
		string vv2 = toString<int64_t>(round(val.number_value()));
		int64_t k = stringToNum<int64_t>(vv2);
		data = new int64_t(stringToNum<int64_t>(vv2));
		int c = 0;
		
	}
	return data;
}



SerializeNode* MsgManager::serialize(string msg, string jdata, SerializeNode*& _outSerials, SerializeNode*& tail)
{

	string err_comment;
	Json data = Json::parse(jdata, err_comment, JsonParse::COMMENTS);
		
	SerializeNode* outSerials = _outSerials;
	stack<Json> dataStack;
	dataStack.push(data);
	SerializeNode* dataNodes = new SerializeNode("message", "head", nullptr);
	outSerials = outSerials->grow("string", "head", new string(msg));
	dataNodes->copy(headMap[msg]->next);
	SerializeNode* cur = dataNodes->next;
	SerializeNode* tcur = dataNodes->next;
	SerializeNode* cacheCur = nullptr;

	while (cur!= NULL)
	{
		index++;
		if (index > 1000) {
			cout <<msg<<" MsgManager::serialize(string msg, string jdata, SerializeNode*& _outSerials, SerializeNode*& tail) error"<<index<<" "<<cur->key <<" "<<jdata<< endl;
			assert(index < 1000);

			break;
		}
		if(index>500){
			cout <<msg<<" MsgManager::serialize(string msg, string jdata, SerializeNode*& _outSerials, SerializeNode*& tail) error"<<index<<" "<<cur->key << endl;

		}
		if (!cur->isRepeated) {
			if (cur->type == "message") {
				string val = *(string*)cur->getVal();
				dataStack.push(dataStack.top()[val]);
				cur = cur->next;
				continue;
			}
			if (cur->type == "int" || cur->type == "float" || cur->type == "string" || cur->type == "long") {
				outSerials = outSerials->grow(cur->type, cur->key, parseData(cur->type, dataStack.top()[cur->key]));
				cur->setVal(parseData(cur->type, dataStack.top()[cur->key]));
				cur = cur->next;
				continue;
			}
			if (cur->type == "end") {
				
				if (dataStack.size() > 0) {
					
					dataStack.pop();
				}
				if (cacheCur != nullptr) {
					
					cur->next = cacheCur;
					cacheCur = nullptr;
				}
				cur = cur->next;
				continue;
			}
	
			if (contains(cur->type,"repeated")) {
				vector<string> typeClass = split(cur->type, ":");
				string tp = trim(typeClass[1]);
				
				int len = *(int*)cur->getVal();
				if (tp == "int" || tp == "float" || tp == "string"|| tp == "long") {
					for (int i = 0; i < len; i++) {
						outSerials = outSerials->grow(tp,"", parseData(tp, dataStack.top().array_items()[i]));
						cur = cur->grow(tp, "", parseData(cur->type, dataStack.top().array_items()[i]));
					}
				}
				else {
					
					for (int i = 0; i < len; i++) {
						SerializeNode* cache = nullptr;
						cur->next = serialize(tp, dataStack.top().array_items()[i].dump(),outSerials,cache);
						cur = cur->next;
						outSerials = cache;
					}
					
				}

				dataStack.pop();
				cur->next = cacheCur;
				cur = cur->next;
				cacheCur = nullptr;
				continue;
			}

			string tpKey = cur->key;
			cacheCur = cur->next;
			cur->copy(headMap[cur->type]);
			cur = cur->next;
			cur->type = "message";
			cur->setVal(new string(tpKey));
		}
		else {
			vector<Json> arr = dataStack.top()[cur->key].array_items();
			dataStack.push(dataStack.top()[cur->key]);
			cacheCur = cur->next;
			cur = cur->grow("repeated:"+cur->type,cur->key,new int(arr.size()));
			outSerials = outSerials->grow("int", "repeated", new int(arr.size()));
		}
	}
	tail = outSerials;
	return dataNodes->next;
}

queue<DynamicNode> MsgManager::serialize(string msg, string jdata)
{

	SerializeNode* dd = new  SerializeNode("head", "msg", 0);
	SerializeNode* tail = nullptr;

	SerializeNode* head = serialize(msg, jdata, dd, tail);
	dd = dd->next;
	SerializeNode* ndd = dd;
	map<string, int> posMap;
	queue<DynamicNode> dq;
	
	posMap["int"] = -1;
	posMap["float"] = -1;

	posMap["string"] = -1;
	posMap["long"] = -1;

	bool addHead = false;
	// cout << "MsgManager::serialize(string msg, string jdata) error"<<index << endl;
	while (dd != nullptr)
	{

		
		if (dd->key == "head") {
			if (!addHead) {
				addHead = true;
			}
			else {
				dd = dd->next;
				continue;
			}
		}
		index++;
		if (index > 1000) {
			cout << "MsgManager::serialize(string msg, string jdata) error"<<index << endl;
			for(int i =0;i<100;i++){
				cout<<ndd->key<<" ";
				ndd = ndd->next;
			}
			assert(index < 1000);
			break;
		}

		DynamicNode dn;
		dn.type = dd->type;
		
		
		if (dn.type == "int") {
			posMap[dn.type]++;
			dn.ival = *(int*)dd->getVal();
		}
		if (dn.type == "float"){
			posMap[dn.type]++;
			dn.fval = *(float*)dd->getVal();
		}
		if (dn.type == "string") {
			
			posMap[dn.type]++;
			dn.sval = *(string*)dd->getVal();
		}
		
		if (dn.type == "long") {
			posMap[dn.type]++;
			dn.lval = *(int64_t*)dd->getVal();
		}
		if (dn.type == "repeated") {
			posMap["int"]++;
			dn.ival = *(int*)dd->getVal();
		}
		dn.posMap = posMap;
		dq.push(dn);
		dd = dd->next;
	}
	SerializeNode::clear();
	return dq;
}
string MsgManager::deSerialize(Layer& layer) {
	
	map<string, int> maps;
	maps["int"] = 0;
	maps["float"] = -1;
	maps["string"] = 0;
	maps["long"] = -1;
	string protoName = layer.SArr[0];
	SerializeNode* head = new SerializeNode("head", "msg", 0);
	head->copy(headMap[protoName]->next);
	SerializeNode* growData = new SerializeNode("string", "head", new string(layer.SArr[0]));
	SerializeNode* thead = growData;
	deSerialize(layer, maps,head,growData);
	Json json;
	stringstream ss;
	ss << "{";
	while (thead != nullptr) {

		index++;

		if (index > 1000) {
			cout << "MsgManager::deSerialize(Layer& layer) error"<<index << endl;
			assert(index < 1000);
			break;
		}
		if (thead->key == "object_begin") {
			ss << "\"" << thead->valToString() << "\":";
			//thead = thead->next;
			ss << deSerialize(thead);
			if (thead != nullptr) {
				ss<<",";
			}
			continue;
		}
		if (thead->key == "end") {

			ss << "}";
			if (thead->next != nullptr && thead->next->key != "end") {
				ss << ",";
			}
			thead = thead->next;
			break;
		}
		if (thead->key == "head") {
			thead = thead->next;
			continue;
		}
		if (thead->key == "arr") {
			thead = thead->next;
			continue;
		}
		if (thead->key == "arr_begin") {
			ss <<"\""<<thead->valToString()<<"\":[";
			thead = thead->next;
			while (thead != nullptr && thead->key != "arr_end") {

				string preType = thead->type;
				if (preType == "int" || preType == "float"|| preType == "long") {
					ss << thead->valToString();
					thead = thead->next;
				}
				if (preType == "string") {
					ss <<"\""<< thead->valToString()<<"\"";
					thead = thead->next;
				}
				if (headMap.count(preType) != 0) {
					//head = head->next;
					ss << deSerialize(thead);
				}
				if (thead != nullptr && thead->key != "arr_end") {
					ss << ",";
				}
			}
			ss << "]";
			if (thead != nullptr && (thead->key != "end"&&thead->key != "arr_end")) {
				ss << ",";
			}
			thead = thead->next;
			continue;
		
		}
		if (thead->type == "int") {

			ss << "\"" << thead->key << "\"" << ":" << thead->valToString();
		}
		if (thead->type == "float") {
			ss << "\"" << thead->key << "\"" << ":" << thead->valToString();
		}
		if (thead->type == "long") {
			ss << "\"" << thead->key << "\"" << ":" << thead->valToString();
		}
		if (thead->type == "string") {
			ss << "\"" << thead->key << "\"" << ":" << "\"" << thead->valToString() << "\"";
		}
		if (thead->next != nullptr && thead->next->key != "end") {
			ss<< ",";
		}
		thead = thead->next;
	}
	ss << "}";
	return ss.str();
}
string MsgManager::deSerialize(SerializeNode*& thead)
{
	stringstream ss;
	
	ss << "{";
	while (thead != nullptr) {
		index++;
		
		if (index > 1000) {
			cout << "MsgManager::deSerialize(SerializeNode*& thead) error" <<index<< endl;
			assert(index < 1000);
			break;
		}

		if (thead->key == "object_begin") {
			
			thead = thead->next;
			continue;
		}
		if (thead->key == "object_end") {
			thead = thead->next;
			break;
		}
		if (thead->key == "head") {
			thead = thead->next;
			continue;
		}
		if (thead->type == "int") {
			ss << "\"" << thead->key << "\"" << ":" << thead->valToString();
		}
		if (thead->type == "float") {
			ss << "\"" << thead->key << "\"" << ":" << thead->valToString();
		}
		if (thead->type == "long") {
			ss << "\"" << thead->key << "\"" << ":" << thead->valToString();
		}
		if (thead->type == "string") {
			ss << "\"" << thead->key << "\"" << ":" << "\"" << thead->valToString() << "\"";
		}
		if (thead->next != nullptr && thead->next->key != "object_end") {
			ss << ",";
		}
		thead = thead->next;
	}
	ss << "}";
	return ss.str();
}
SerializeNode* MsgManager::deSerialize(Layer layer, map<string, int>& maps, SerializeNode*& info, SerializeNode*& growData)
{
	SerializeNode* head = info->next;
	SerializeNode* thead = growData;
	SerializeNode* lastNodes = nullptr;
	while (head != nullptr)
	{
		index++;
		if (index > 1000) {
			cout << "MsgManager::deSerialize(Layer layer, map<string, int>& maps, SerializeNode*& info, SerializeNode*& growData) error"<<index << endl;
			assert(index < 1000);
			break;
		}
		if (!head->isRepeated) {

			if (head->type == "int") {
				maps[head->type]++;
				growData = growData->grow(head->type, head->key, new int(layer.IArr[maps[head->type]]));
				head = head->next;
				continue;
			}
			if (head->type == "float") {
				maps[head->type]++;
				growData = growData->grow(head->type, head->key, new float(layer.FArr[maps[head->type]]));
				head = head->next;
				continue;
			}
			if (head->type == "string") {
				maps[head->type]++;
				growData = growData->grow(head->type, head->key, new string(layer.SArr[maps[head->type]]));
				head = head->next;
				continue;
			}
			if (head->type == "long") {
				maps[head->type]++;
				growData = growData->grow(head->type, head->key, new int64_t(layer.LArr[maps[head->type]]));
				head = head->next;
				continue;
			}
			if (head->type == "end") {

				if (lastNodes != nullptr) {
					growData = growData->grow("string", "object_end", new string(head->key));
					head->next = lastNodes;
					lastNodes = lastNodes->next;
					head = head->next;
					continue;
				}
				else {
					break;
				}
			}
			growData = growData->grow("string", "object_begin", new string(head->key));
			lastNodes = head->next;
			head->copy(headMap[head->type]->next);
		}
		else {
			growData = growData->grow("string", "arr_begin", new string(head->key));
			maps["int"]++;
			int len = layer.IArr[maps["int"]];
			string type = head->type;
			for (int i = 0; i < len; i++) {
				if (type == "int") {
					maps["int"]++;
					growData = growData->grow("int", "", new int(layer.IArr[maps["int"]]));
					continue;
				}
				if (type == "float") {
					maps["float"]++;
					growData = growData->grow("float", "", new float(layer.FArr[maps["float"]]));
					continue;
				}
				if (type == "long") {
					maps["long"]++;
					growData = growData->grow("long", "", new int64_t(layer.LArr[maps["long"]]));
					continue;
				}
				if (type == "string") {
					maps["string"]++;
					growData = growData->grow("string", "arr_string", new string(layer.SArr[maps["string"]]));
					continue;
				}
				if (headMap.count(type) != 0) {
					lastNodes = head->next;
					head->copy(headMap[type]);
					growData = growData->grow(type, "object_begin", 0);
					head = head->next;
					deSerialize(layer, maps, head,growData);
					growData = growData->grow(type, "object_end", 0);
					head->next = lastNodes;
					continue;
				}
			}
			head = head->next;
			growData = growData->grow("string", "arr_end", new string(head->key));
		}
		head = head->next;
	}

	return thead;
}
vector<string> MsgManager::Buffer2Json(string buf)
{
	index = 0;
	vector<string> vv;
	Layer la;
	la.fromBuffer(buf);
	vv.push_back(deSerialize(la));
	vv.push_back(la.SArr[0]);
	return vv;
}
string MsgManager::Json2Buffer(string msgType, string json)
{
	index = 0;
	Layer la = serializeToLayer(msgType, json);

	return la.toBuffer();
}

Layer MsgManager::serializeToLayer(string msg, string jdata)
{
	preProcess = jdata;
	Layer layer;
	layer.init(msg);
	layer.SArr.clear();
	queue<DynamicNode> qq = serialize(msg, jdata);
	while (qq.size() != 0)
	{
		if (qq.front().type == "int") {
			layer.IArr.push_back(qq.front().ival);
		}
		if (qq.front().type == "float") {
			layer.FArr.push_back(qq.front().fval);
		}
		if (qq.front().type == "string") {
			layer.SArr.push_back(qq.front().sval);
		}
		if (qq.front().type == "long") {
			layer.LArr.push_back(qq.front().lval);
		}
		qq.pop();
	}
	return layer;
}
