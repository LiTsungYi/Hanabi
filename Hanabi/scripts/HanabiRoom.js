"use strict";

function RequestRoomList()
{
    var payload = null;
    SendCommand( "GetRoomList", payload );
}

function ResponseRoomList( payload )
{
    $( "#RoomListItems" ).empty();
    for ( var count = 0; count < payload.Rooms.length; ++ count )
    {
        var roomInfo = payload.Rooms[ count ];
        var roomItem = $( "#TemplateRoomItem" ).clone();
        roomItem.attr( "id", "RoomItem_" + roomInfo.RoomIndex ).removeClass( "Hided" );
        
        var roomIndexNode = $( "#TemplateRoomIndex" ).clone();
        roomIndexNode.removeClass( "Hided" ).text( roomInfo.RoomIndex );
        roomItem.append( roomIndexNode );
        
        for ( var playerCount = 0; playerCount < roomInfo.Players.length; ++ playerCount )
        {
            var playerNickname = roomInfo.Players[ playerCount ];
            var playerNode = $( "#TemplateRoomItemPlayer" ).clone();
            
            if ( IsMe( playerNickname ) )
            {
                playerNode.addClass( "label-success" );
            }
            else
            {
                playerNode.addClass( "label-primary" );
            }
            playerNode.removeClass( "Hided" ).text( playerNickname );
            
            roomItem.append( playerNode );
        }
        
        if ( roomInfo.RoomIndex == joinedRoomIndex )
        {
            var buttonNode = $( "#TemplateQuitRoom" ).clone();
            buttonNode.removeClass( "Hided" );
            buttonNode.click( function() { RequestQuitRoom(); } );
            roomItem.append( buttonNode );
        }
        else
        {
            var buttonNode = $( "#TemplateJoinRoom" ).clone();
            buttonNode.removeClass( "Hided" );
            buttonNode.attr( "data-RoomIndex", roomInfo.RoomIndex );
            buttonNode.click( function() { RequestJoinRoom( this.getAttribute( "data-RoomIndex" ) ); } );
            roomItem.append( buttonNode );
        }
        
        $( "#RoomList ul:first" ).append( roomItem );
    }
}

function RequestJoinRoom( roomIndex )
{
    var payload = 
    {
        roomIndex : roomIndex
    };
    
    SendCommand( "JoinRoom", payload );
}

function ResponseJoinRoom( payload )
{
    if ( payload.Result == "0" ) // Success
    {
        joinedRoomIndex = payload.Room.RoomIndex;
		
        var playerNode = $( "#TemplateRoomItemPlayer" ).clone();
        playerNode.removeClass( "Hided" ).text( $( "#nickname" ).val() );
        $( "#RoomItem_" + joinedRoomIndex ).append( playerNode );
        
        $( "#RoomListReady" ).removeClass( "Hided" );
    }
    
    RequestRoomList();
}

function RequestQuitRoom()
{
    var payload = 
    {
        roomIndex : joinedRoomIndex
    };
    
    SendCommand( "QuitRoom", payload );
}

function ResponseQuitRoom( payload )
{
    if ( payload.Result == "0" ) // Success
    {
        var quitRoomIndex = payload.Room.RoomIndex;
        joinedRoomIndex = -1;
        
		if ( $( "#RoomList.Hided" ) != null )
		{
			$( "#RoomItem_" + quitRoomIndex ).remove();
			$( "#RoomListReady" ).addClass( "Hided" );
			$( "#PlayerReadyButton" ).removeClass( "disabled" );
		}
		else if ( $( "#GameBoard.Hided" ) != null )
		{
			$( "#GameBoard" ).addClass( "Hided" );
			$( "#RoomList" ).removeClass( "Hided" );
		}
    }
    
    RequestRoomList();
}

function RequestReady()
{
    SendCommand( "Ready", null );
}

function ResponseReady()
{
    $( "#PlayerReadyButton" ).addClass( "disabled" );
}

function ResponseTurn( payload )
{
	$( ".Player" ).removeClass( "ActivePlayer" );
	$( "#Player_" + payload.Nickname ).addClass( "ActivePlayer" );
}

function ResponseRound( payload )
{
	$( "#GameScore" ).text( payload.Score );
	$( "#GameScoreModel" ).modal( "show" );
}
