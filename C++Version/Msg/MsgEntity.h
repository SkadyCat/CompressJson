


#pragma once
#include <map>
#include <string>
#include <sstream>
#include <vector>
#include <queue>
#include <unordered_map> 
#include <iostream>
#include <cassert>
#include <cmath>
const std::string WHITESPACE = " \n\r\t\f\v";
using namespace std;
static int valIndex = 0;

class Layer {

typedef union
{
	float f;
	unsigned char uc[4];
} FUN;
public:
	vector<int> IArr;
	vector<float> FArr;
	vector<string> SArr;
	vector<int64_t> LArr;
	vector<char> chars;

	Layer() {
		
	}
	
	void init(string msgField) {
		IArr.push_back(0);
		SArr.push_back(msgField);
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
	string ltrim(const std::string& s)
	{
		size_t start = s.find_first_not_of(WHITESPACE);
		return (start == std::string::npos) ? "" : s.substr(start);
	}

	string rtrim(const std::string& s)
	{
		size_t end = s.find_last_not_of(WHITESPACE);
		return (end == std::string::npos) ? "" : s.substr(0, end + 1);
	}

	string trim(const std::string& s) {
		return rtrim(ltrim(s));
	}

	int charsToInt(vector<char> vv)
	{

		int v = 0;
		int len = vv[0];
		if (len >= 4) {
			len = len - 4;
		}
		for (int i = 0; i < len+1; i++) {
			unsigned char t = (unsigned char)vv[i + 1];
			v = v | (t << 8 * i);
		}
		if (vv[0] >= 4) {
			return -v;
		}
		return v;
	}

	vector<char> int2bytes(int v)
	{
		int flag = v;
		
		vector<char> aim;
		v = abs(v);
		vector<char> chars(4);
		chars[3] = (char)(v >> 24);
		chars[2] = (char)(v >> 16);
		chars[1] = (char)(v >> 8);
		chars[0] = (char)v;
		if (v < 256)
		{
			if (flag < 0) {
				aim.push_back(0x04);
			}
			else {
				aim.push_back(0x00);
			}
		}
		if (v >= 256 && v < 65536)
		{
			if (flag < 0) {
				aim.push_back(0x05);
			}
			else {
				aim.push_back(0x01);
			}
		}
		if (v >= 65536 && v < (256 * 256 * 256))
		{
			if (flag < 0) {
				aim.push_back(0x06);
			}
			else {
				aim.push_back(0x02);
			}
		}
		if (v >= (256 * 256 * 256))
		{
			if (flag < 0) {
				aim.push_back(0x07);
			}
			else {
				aim.push_back(0x03);
			}
		}


		switch (aim[0])
		{
		case 0x00:
			aim.push_back(chars[0]);
			break;

		case 0x01:
			aim.push_back(chars[0]);
			aim.push_back(chars[1]);
			break;

		case 0x02:
			aim.push_back(chars[0]);
			aim.push_back(chars[1]);
			aim.push_back(chars[2]);
			break;

		case 0x03:
			aim.push_back(chars[0]);
			aim.push_back(chars[1]);
			aim.push_back(chars[2]);
			aim.push_back(chars[3]);
			break;
		case 0x04:
			aim.push_back(chars[0]);
			break;

		case 0x05:
			aim.push_back(chars[0]);
			aim.push_back(chars[1]);
			break;

		case 0x06:
			aim.push_back(chars[0]);
			aim.push_back(chars[1]);
			aim.push_back(chars[2]);
			break;

		case 0x07:
			aim.push_back(chars[0]);
			aim.push_back(chars[1]);
			aim.push_back(chars[2]);
			aim.push_back(chars[3]);
			break;

		}
		return aim;
	}
	vector<char> int2char4(int v)
	{
		vector<char> chars(4);
		chars[3] = (char)(v >> 24);
		chars[2] = (char)(v >> 16);
		chars[1] = (char)(v >> 8);
		chars[0] = (char)v;
		return chars;
	}
	vector<char> string2byte(string v) {

		const char* tp = v.c_str();

		vector<char> lb;
		vector<char> tc = int2char4(v.size());
		int len = v.size();
		if (len < 256)
		{
			lb.push_back(0x00);
			lb.push_back(tc[0]);
		}
		if (len > 256 && len < 256 * 256)
		{
			lb.push_back(0x01);
			lb.push_back(tc[0]);
			lb.push_back(tc[1]);
		}
		if (len > 256 * 256 && len < 256 * 256 * 256)
		{
			lb.push_back(0x02);
			lb.push_back(tc[0]);
			lb.push_back(tc[1]);
			lb.push_back(tc[2]);
		}
		if (len > 256 * 256 * 256)
		{
			lb.push_back(0x03);
			lb.push_back(tc[0]);
			lb.push_back(tc[1]);
			lb.push_back(tc[2]);
			lb.push_back(tc[3]);
		}

		for (int i = 0; i <len; i++)
		{
			lb.push_back(tp[i]);
		}
		return lb;
	}
	vector<char> long2bytes(int64_t v)
	{

		vector<char> chars(8);
		chars[7] = (char)(v >> 56);
		chars[6] = (char)(v >> 48);
		chars[5] = (char)(v >> 40);
		chars[4] = (char)(v >> 32);
		chars[3] = (char)(v >> 24);
		chars[2] = (char)(v >> 16);
		chars[1] = (char)(v >> 8);
		chars[0] = (char)v;

		vector<char> aim;
		if (v < pow(256, 1))
		{
			aim.push_back(0x00);
		}
		if (v >= pow(256, 1) && v < pow(256, 2))
		{
			aim.push_back(0x01);

		}
		if (v >= pow(256, 2) && v < pow(256, 3))
		{
			aim.push_back(0x02);
		}
		if (v >= pow(256, 3) && v < pow(256, 4))
		{
			aim.push_back(0x03);

		}
		if (v >= pow(256, 4) && v < pow(256, 5))
		{

			aim.push_back(0x04);
		}
		if (v >= pow(256, 5) && v < pow(256, 6))
		{

			aim.push_back(0x05);
		}
		if (v >= pow(256, 6) && v < pow(256, 7))
		{

			aim.push_back(0x06);
		}
		if (v >= pow(256, 7))
		{
			aim.push_back(0x07);
		}
		int len = (int)aim[0];
		for (int i = 0; i < len + 1; i++)
		{
			aim.push_back(chars[i]);
		}
		return aim;
	}

	int64_t bytes2long(vector<char> buf)
	{
		//3333333333333333
		//3333333333333333
		//3333333333333333
		int len = buf[0];
		int64_t v = 0;
		for (int i = 1; i <= len +1; i++)
		{
			int64_t a = (unsigned char)buf[i];
			v = v | (a << (8 * (i-1)));
		}
		return v;
	}
	string toBuffer()
	{

		assert(IArr.size() != 0);

		
		queue<char> q;
		setQueue(q, string2byte(SArr[0]));
		vector<char> iarrbytes;
		vector<char> farrbytes;
		vector<char> larrbytes;
		vector<char> sarrbytes;

		for (int i = 1; i < IArr.size(); i++) {
			vector<char> arr = int2bytes(IArr[i]);
			for(auto k : arr) {
				iarrbytes.push_back(k);
			}
		}


		for (int i = 0; i < LArr.size(); i++)
		{
			vector<char> arr = long2bytes(LArr[i]);
			for(auto k : arr)
			{
				larrbytes.push_back(k);
			}
		}
		
		//float v = bytes2long(larrbytes.po);
		for (int i = 1; i < SArr.size(); i++)
		{
			vector<char> arr = string2byte(SArr[i]);
			for (auto k : arr)
			{
				sarrbytes.push_back(k);
			}
		}

		for (int i = 0; i < FArr.size(); i++)
		{
			vector<char> arr = float2char(FArr[i]);
			for (auto k : arr){
				farrbytes.push_back(k);

			}
		}

		
		
		setQueue(q, int2bytes(IArr.size() - 1));
		setQueue(q, int2bytes(LArr.size()));
		setQueue(q, int2bytes(SArr.size() - 1));
		setQueue(q, int2bytes(FArr.size()));
		setQueue(q, iarrbytes);
		setQueue(q, larrbytes);
		setQueue(q, sarrbytes);
		setQueue(q, farrbytes);

		string buf;
		while(q.size()!= 0){
			buf.push_back(q.front());
			q.pop();
		}
		return buf;
	}
	vector<char> extractFromQueue(queue<char>& q)
	{
		vector<char> tp;
		tp.push_back(q.front());
		q.pop();
		int len = tp[0];
		if (len >= 4) {
			len = len - 4;
		}
		for (int i = 0; i < len + 1; i++)
		{
			tp.push_back(q.front());
			q.pop();
		}
		return tp;
	}

	vector<char> extractFromQueue2(queue<char>& q)
	{
		vector<char> tp;
		int len = charsToInt(extractFromQueue(q));

		for (int i = 0; i < len; i++)
		{
			tp.push_back(q.front());
			q.pop();
		}
		return tp;
	}
	vector<char> extractFromQueue3(queue<char>& q)
	{
		vector<char> tp;
		int len = 4;
		for (int i = 0; i < len; i++)
		{
			tp.push_back(q.front());
			q.pop();
		}
		return tp;
	}

	Layer fromBuffer(string buff)
	{
		queue<char> q;
		string head = "";

		
		//0a27829d41
		//a27829d411
		for (int i = 0; i < buff.size(); i++) {
			q.push(buff[i]);
		}
		int hlen = charsToInt(extractFromQueue(q));

		for (int i = 0; i < hlen; i++) {
			head += q.front();
			q.pop();
		}
		vector<char> tbuf;
		int type =0;
		IArr.push_back(0);
		SArr.push_back(head);
		tbuf = extractFromQueue(q);
		type = charsToInt(tbuf);
		int iarrLen = type;

		tbuf = extractFromQueue(q);
		type = charsToInt(tbuf);
		int larrLen = type;

		tbuf = extractFromQueue(q);
		type = charsToInt(tbuf);
		int sarrLen = type;

		tbuf = extractFromQueue(q);
		type = charsToInt(tbuf);
		int farrLen = type;

		for (int i = 0; i < iarrLen; i++) {
			int v = charsToInt(extractFromQueue(q));
			IArr.push_back(v);
		}
		for (int i = 0; i < larrLen; i++)
		{
			int64_t v = bytes2long(extractFromQueue(q));
			LArr.push_back(v);
		}
		for (int i = 0; i < sarrLen; i++)
		{
			vector<char> v = extractFromQueue2(q);
			string vs ;

			for (auto k : v) {

				vs.push_back(k);
			}
			SArr.push_back(vs);
		}
		for (int i = 0; i < farrLen; i++)
		{
			vector<char> v = extractFromQueue(q);
			FArr.push_back(charToFloat(v));
		}

		return *this;
	}

	queue<char> convertBuffer(string buff) {

		queue<char> cq;
		for (auto k : buff)
		{
			int v;
			stringstream ss;
			cq.push(k);
		}
		return cq;
	}
	//vector<char> extractFromQueue(queue<char>& q) {
	//	
	//	vector<char> cc;
	//	for (int i = 0; i < 4; i++) {
	//		cc.push_back(q.front());
	//		q.pop();
	//	}
	//	return cc;
	//}

	void setQueue(queue<char>& q,const vector<char>& data) {
		for (auto k : data) {
			q.push(k);
		}
	}

	//Layer fromBuffer(string buf) {
	//	queue<char> q = convertBuffer(buf);
	//	return convertBuffer(q);
	//}
	//Layer convertBuffer(queue<char>& q) {
	//	vector<char> _iarrLen = extractFromQueue(q);
	//	vector<char> _farrLen = extractFromQueue(q);
	//	vector<char> _sarrLen = extractFromQueue(q);
	//	int iarrLen = charsToInt(_iarrLen);
	//	int farrLen = charsToInt(_farrLen);
	//	int sarrLen = charsToInt(_sarrLen);
	//	for (int i = 0; i < iarrLen; i++) {
	//		int v = charsToInt(extractFromQueue(q));
	//		IArr.push_back(v);
	//	}
	//	for (int i = 0; i < farrLen; i++) {
	//		float v = charToFloat(extractFromQueue(q));
	//		FArr.push_back(v);
	//	}
	//	
	//	for (int i = 0; i < sarrLen; i++) {
	//		int v = charsToInt(extractFromQueue(q));
	//		string vs = "";
	//		for (int j = 0; j < v; j++) {
	//			vs += q.front();
	//			q.pop();
	//		}
	//		SArr.push_back(vs);
	//	}
	//	return *this;
	//}

	unsigned int ra;
	float test;
	char result[4];
	FUN fun;



	void charRepresentation(char* uc, float f)
	{
		FUN fun;

		fun.f = f;
		uc[0] = fun.uc[3];
		uc[1] = fun.uc[2];
		uc[2] = fun.uc[1];
		uc[3] = fun.uc[0];
	}

	void floatRepresentation(char* uc, float* f)
	{
		FUN fun;
		fun.uc[3] = uc[0];
		fun.uc[2] = uc[1];
		fun.uc[1] = uc[2];
		fun.uc[0] = uc[3];
		*f = fun.f;
	}

	float charToFloat(vector<char> data) {
		/*for (int ra = 0; ra < 4; ra++) {
			result[ra] = data[ra];
		}
		floatRepresentation(result, &test);*/

		int v = charsToInt(data);
		return ((float)v)/(1000.0F);
	}

	vector<char> float2char(float v) {
		/*vector<char> vec;
		test = v;
		charRepresentation(result, test);
		for (int ra = 0; ra < 4; ra++) {
			vec.push_back(result[ra]);
		}*/
		int v1 = v * 1000;

		return int2bytes(v1);
	}
	void clear() {
		IArr.clear();
		FArr.clear();
		SArr.clear();
	}
	string ToString() {
		stringstream ss;
		ss << "IArr:";
		for (int i = 0; i < IArr.size(); i++) {
			ss << "[" << i << "](" << IArr[i] << ") ";
		}
		ss << endl;

		ss << "FArr:";
		for (int i = 0; i < FArr.size(); i++) {
			ss << "[" << i << "](" << FArr[i] << ") ";
		}
		ss << endl;

		ss << "SArr:";
		for (int i = 0; i < SArr.size(); i++) {
			ss << "[" << i << "](" << SArr[i] << ") ";
		}
		ss << endl;

		ss << "LArr:";
		for (int i = 0; i < LArr.size(); i++) {
			ss << "[" << i << "](" << LArr[i] << ") ";
		}
		ss << endl << endl;
		return ss.str();
	}
};

struct DynamicNode {
	string type;
	map<string, int> posMap;
	int ival;
	string sval;
	float fval;
	int64_t lval;
	int getIndex() {
		if (type == "repeated") {
			return posMap["int"];
		}
		return posMap[type];
	}
	string toVal() {
		stringstream ss;
		if (type == "int") {
			ss << ival;
			return ss.str();
		}
		if (type == "float") {
			ss << fval;
			return ss.str();
		}
		if (type == "string") {
			ss << sval;
			return ss.str();
		}
		ss << ival;
		return ss.str();
	}
};

struct SerializeNode
{
private:
	void* val;
public:
	static int pointer;
	static vector<SerializeNode*> keyMap;
	static void clear() {
		vector<SerializeNode*> cache;
		for(SerializeNode* k: keyMap)
		{
			if (k->isDynamic) {
				delete k;
			}
			else
			{
				cache.push_back(k);
			}
		}
		pointer -= (keyMap.size() - cache.size());
		valIndex -= (keyMap.size() - cache.size());
		keyMap.clear();
		keyMap = cache;
	}
	int size;
	map<string, int> maps;
	SerializeNode* next;
	string type;
	string key;
	SerializeNode* parent;
	string dataKey;
	bool isRepeated;
	int repeatedNum;
	bool isDynamic;
	void setVal(void* val) {
		if (this->val == nullptr) {
			this->val = val;
			
		}
		else {
			clearNode();
			this->val = val;
		}
	}
	void* getVal() {
		
		return this->val;
	}

	string valToString() {
		stringstream ss;
		if (type == "int") {
			ss << *(int*)val;
			return ss.str();
		}
		if (type == "float") {
			ss << *(float*)val;
			return ss.str();
		}
		if (type == "string") {
			ss << *(string*)val;
			return ss.str();
		}
		if (type == "long") {
			ss << *(int64_t*)val;
			return ss.str();
		}
		ss << *(int*)val;
		return ss.str();
	}
	SerializeNode(string type,string key,void* data) {
		valIndex++;
		pointer++;
		this->type = type;
		next = nullptr;
		parent = nullptr;
		this->key = key;
		this->isRepeated = false;

		this->val = data;
		isDynamic = true;
		SerializeNode::keyMap.push_back(this);
	}
	SerializeNode(SerializeNode* node) {
		pointer++;
		key = node->key;
		type = node->type;
		parent = nullptr;
		next = nullptr;
		val = nullptr;
		maps = node->maps;
		isRepeated = node->isRepeated;
		valIndex++;
		isDynamic = true;
		SerializeNode::keyMap.push_back(this);
	}
	SerializeNode* grow(string type,string key,void* data) {
		SerializeNode* nNode = new SerializeNode(type,key,data);
		nNode->parent = this;
		this->next = nNode;
		nNode->maps = this->maps;
		nNode->maps[type] = this->maps[type] + 1;
		return nNode;
	}

	SerializeNode* copy(SerializeNode* node) {
		SerializeNode* cur = node;
		SerializeNode* pt = this;
		SerializeNode* tailPoint = nullptr;
		while (cur != nullptr) {
			SerializeNode* tp = new SerializeNode(cur);
			tp->parent = pt;
			pt->next = tp;
			pt = pt->next;
			tailPoint = cur;
			cur = cur->next;
		}
		return tailPoint;

	}

	void clearNode() {
		
		if (type == "int") {
			delete (int*)val;
			return;
		}
		if (type == "float") {
			delete (float*)val;
			return;
		}
		if (type == "string") {
			delete (string*)val;
			return;
		}

		if (type == "message") {
			
			delete (string*)val;

			return;
		}
		delete (int*)val;
	}
	~SerializeNode(){
		
		clearNode();
	}
};
