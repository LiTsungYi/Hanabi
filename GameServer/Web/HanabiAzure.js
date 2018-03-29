"use strict";

var ws = null;
var wsImpl = window.WebSocket || window.MozWebSocket;
var webConnection = "Disconnected";

var badRequest = "400";
var notFound = "404";

var joinedRoomIndex = -1;

function Connect()
{
    ws = new wsImpl( "ws://litsungyi.cloudapp.net:3033/", "Hanabi" );
    
    ws.onmessage = function( evt )
    {
        DispatchMessage( evt.data );
    };

    ws.onopen = function()
    {
        webConnection = "Connected";
    };

    ws.onclose = function()
    {
        webConnection = "Disconnected";
        ws = null;
        alert( "連線中斷，請重新整理" );
    };
}

$( "body" ).ready( function() {
    $( "#EnterGameButton" ).click( function() { RequestEnterGame(); } );
    $( "#ExitGameButton" ).click( function() { RequestExitGame(); } );
    $( "#ReadyButton" ).click( function() { RequestReady(); } );
    
    $( "#RefreshRoomListButton" ).click( function() { RequestRoomList(); } );
    $( "#PlayerReadyButton" ).click( function() { RequestReady(); } );
    
	$( "#CloseNotifyGame" ).click( function() { RequestQuitRoom(); } );
	
    Connect();
} );

function RequestEnterGame()
{
    var payload = 
    {
        nickname : $( "#nickname" ).val()
    };
    SendCommand( 'EnterGame', payload );
}

function ResponseEnterGame( payload )
{
    if ( "0" == payload.Result )
    {
        $( "#EnterGame" ).addClass( "Hided" );
        $( "#ExitGame" ).removeClass( "Hided" );
        $( "#nicknameSpan" ).text( $( "#nickname" ).val() );
        $( "#nickname" ).val( "" );
        
        $( "#RoomList" ).removeClass( "Hided" );
    }
    
    RequestRoomList();
}

function RequestExitGame()
{
    var payload = 
    {
        nickname : $( "#nickname" ).val()
    };
    SendCommand( 'ExitGame', payload );
}

function ResponseExitGame( payload )
{
    if ( "0" == payload.Result )
    {
        $( "#EnterGame" ).removeClass( "Hided" );
        $( "#ExitGame" ).addClass( "Hided" );
		$( "#RoomList" ).addClass( "Hided" );
		$( "#GameBoard" ).addClass( "Hided" );
		document.refresh();
    }
	joinedRoomIndex = -1;
}

function SendCommand( action, payload )
{
    "use strict";
    
    if ( webConnection == "Disconnected" )
    {
        alert( "連線中斷，請重新整理" );
        return;
    }
    
    var payloadString = JSON.stringify( payload );
    var command = {
        action : action,
        payload : payloadString
    };
    
    var commandString = JSON.stringify( command );
    ws.send( commandString );
}

var lastResponse;
var lastAction;
var lastPayload;
function DispatchMessage( data )
{
    "use strict";
    
    var response = JSON.parse( data );
    var payload = JSON.parse( response.Payload );
    var action = response.Action;
	
	lastResponse = response;
	lastAction = action;
	lastPayload = response.Payload;
    if  ( "EnterGame" == action )
    {
        ResponseEnterGame( payload );
    }
    else if ( "ExitGame" == action )
    {
        ResponseExitGame( payload );
    }
    else if ( "GetRoomList" == action )
    {
        ResponseRoomList( payload );
    }
    else if ( "JoinRoom" == action )
    {
        ResponseJoinRoom( payload );
    }
    else if ( "QuitRoom" == action )
    {
        ResponseQuitRoom( payload );
    }
    else if  ( "Ready" == action )
    {
        ResponseReady( payload );
    }
    else if ( "NotifyBoard" == action )
    {
        ResponseGameBegin( payload );
    }
	else if ( "NotifyTurn" == action )
	{
        ResponseTurn( payload );
	}
	else if ( "NotifyRound" == action )
	{
        ResponseRound( payload );
	}
    else if ( "PromptCard" == action )
    {
        ResponsePrompt( payload );
    }
    else if ( "PlayCard" == action )
    {
        ResponsePlay( payload );
    }
    else if ( "DiscardCard" == action )
    {
        ResponseDiscard( payload );
    }
    else
    {
        alert( "Not support action type: " + action );
    }
}
