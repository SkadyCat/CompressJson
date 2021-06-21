set(Msg_LIBRARY "Msg")


include_directories(.)

set(Msg_HEADER_FILES
    ${CMAKE_CURRENT_SOURCE_DIR}/MsgEntity.h
	${CMAKE_CURRENT_SOURCE_DIR}/MsgManager.h
	${CMAKE_CURRENT_SOURCE_DIR}/json11.hpp
	
)
set(Msg_SOURCE_FILES	
	${CMAKE_CURRENT_SOURCE_DIR}/Msg.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/MsgEntity.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/MsgManager.cpp
	${CMAKE_CURRENT_SOURCE_DIR}/json11.cpp
)

include_directories( 
	#${LUA_INCLUDE_DIR}
	#${TW2CORE_INCLUDE_DIR}
	${CMAKE_CURRENT_SOURCE_DIR}
	${PROJECT_SOURCE_DIR}/Lua/src
	${PROJECT_SOURCE_DIR}/Msg
)


IF (CMAKE_SYSTEM_NAME MATCHES "Linux")
	
	add_library(${Msg_LIBRARY} SHARED ${Msg_SOURCE_FILES}  ${Msg_HEADER_FILES})

	set_target_properties(${Msg_LIBRARY} PROPERTIES PREFIX "")
	SET(LIBRARY_OUTPUT_PATH ../Share) 
	target_link_libraries(${Msg_LIBRARY} PRIVATE ${LUA_LIBRARY} lua)
	set_target_properties(${Msg_LIBRARY} PROPERTIES PREFIX "")
	set(LIBRARY_OUTPUT_PATH /home/magic/skynets/skynet/luaclib)

	#target_link_libraries(${Msg_LIBRARY} PRIVATE ${LUA_LIBRARY} ${TW2CORE_LIBRARY} lualib)
ENDIF (CMAKE_SYSTEM_NAME MATCHES "Linux")

IF (CMAKE_SYSTEM_NAME MATCHES "Windows")
add_library(${Msg_LIBRARY} SHARED ${Msg_SOURCE_FILES}  ${Msg_HEADER_FILES})

set_target_properties(${Msg_LIBRARY} PROPERTIES PREFIX "")
SET(LIBRARY_OUTPUT_PATH ../Share) 
target_link_libraries(${Msg_LIBRARY} PRIVATE ${LUA_LIBRARY} lua)

	
ENDIF (CMAKE_SYSTEM_NAME MATCHES "Windows")
SET_TARGET_PROPERTIES(${Msg_LIBRARY} PROPERTIES FOLDER "Help")
