local msg = require "Msg"
local processer = msg.new()
msg.load(processer,"F:\\C#Proj\\LCJ\\CompressJson\\C++Version\\build\\Share\\Debug\\msgProto.txt")
local j1 = '{"name":"ljy","equip_id":222}'
local buf = msg.Json2Buffer(processer,"PutOnRequest",j1)
local jv = msg.Buffer2Json(processer,buf)