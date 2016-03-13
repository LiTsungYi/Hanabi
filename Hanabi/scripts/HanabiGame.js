"use strict";

function IsMe( nickname )
{
	return ( nickname == $( "#nicknameSpan" ).text() );
}

function UpdateToken( token )
{
    $( "#RemainedHints" ).text( token.Note );
    $( "#RemainedErrors" ).text( token.Storm );
}

function UpdateDrawPile( count )
{
    $( "#DrawCard" ).text( count );
}

function ResponseGameBegin( payload )
{
    $( "#RoomList" ).addClass( "Hided" );
    $( "#GameBoard" ).removeClass( "Hided" );
	
    UpdateToken( payload.Token );
    UpdateDrawPile( payload.DrawPileCount );
    
    for ( var nickname in payload.Cards )
    {
        CreateCards( nickname, payload.Cards[ nickname ] );
    }
}

function CreateCards( nickname, cards )
{
    var playerNode = $( "#TemplatePlayer" ).clone();
	playerNode.attr( "id", "Player_" + nickname ).removeClass( "Hided" )
		.find( ".NicknameLabel" ).text( nickname );
    $( "#Players" ).append( playerNode );
	
	if ( IsMe( nickname ) )
	{
		playerNode.find( ".NicknameLabel" ).addClass( "label-success" )
		playerNode.find( "div" ).remove();
		UpdateFaceDownCards( nickname, cards );
	}
	else
	{
		playerNode.find( ".NicknameLabel" ).addClass( "label-primary" );
		UpdateFaceUpCards( nickname, cards );
		AppendPromptButton( nickname );
	}
}

function UpdateFaceDownCards( nickname, cards )
{
    var playerNode = $( "#Player_" + nickname );
	playerNode.find( ".FaceDownCard" ).remove();
    for ( var hintCount in cards )
    {
        var handCard = CreateFaceDownCard( cards[ hintCount ] );
		playerNode.append( handCard );
    }
}

function UpdateFaceUpCards( nickname, cards )
{
    var playerNode = $( "#Player_" + nickname );
	playerNode.find( ".FaceUpCard" ).remove();
    
    for ( var card in cards )
    {
        var cardButton = CreateFaceUpCard( cards[ card ] );
		playerNode.find( "div:first" ).append( cardButton );
    }
}

function AppendPromptButton( nickname )
{
    var hintButton = $( "#TemplateHintButton" ).clone();
    hintButton.removeClass( "Hided" ).attr( "id", "HintButton_" + nickname );
    
    hintButton.find( ".HintValue1Tag" ).attr( "data-Nickname", nickname )
        .click( function() { RequestPrompt( this.getAttribute( "data-Nickname" ), 1 ); } );
    hintButton.find( ".HintValue2Tag" ).attr( "data-Nickname", nickname )
        .click( function() { RequestPrompt( this.getAttribute( "data-Nickname" ), 2 ); } );
    hintButton.find( ".HintValue3Tag" ).attr( "data-Nickname", nickname )
        .click( function() { RequestPrompt( this.getAttribute( "data-Nickname" ), 3 ); } );
    hintButton.find( ".HintValue4Tag" ).attr( "data-Nickname", nickname )
        .click( function() { RequestPrompt( this.getAttribute( "data-Nickname" ), 4 ); } );
    hintButton.find( ".HintValue5Tag" ).attr( "data-Nickname", nickname )
        .click( function() { RequestPrompt( this.getAttribute( "data-Nickname" ), 5 ); } );
    
    hintButton.find( ".HintBlueTag" ).attr( "data-Nickname", nickname )
        .click( function() { RequestPrompt( this.getAttribute( "data-Nickname" ), 10 ); } );
    hintButton.find( ".HintGreenTag" ).attr( "data-Nickname", nickname )
        .click( function() { RequestPrompt( this.getAttribute( "data-Nickname" ), 20 ); } );
    hintButton.find( ".HintYellowTag" ).attr( "data-Nickname", nickname )
        .click( function() { RequestPrompt( this.getAttribute( "data-Nickname" ), 30 ); } );
    hintButton.find( ".HintRedTag" ).attr( "data-Nickname", nickname )
        .click( function() { RequestPrompt( this.getAttribute( "data-Nickname" ), 40 ); } );
    hintButton.find( ".HintWhiteTag" ).attr( "data-Nickname", nickname )
        .click( function() { RequestPrompt( this.getAttribute( "data-Nickname" ), 50 ); } );
    
    var playerNode = $( "#Player_" + nickname );
    playerNode.append( hintButton );
}

function CreateFaceDownCard( hint )
{
	var cardIndex = hint.Index;
    var handCard = $( "#TemplateFaceDownCard" ).clone();
    $( handCard ).removeClass( "Hided" ).attr( "id", "Card_" + cardIndex );
    $( handCard ).find( "button:first" ).text( "#" + cardIndex );
    
    var hintMessage = "";
    if ( hint.Prompt.Color != "0" )
	{
        hintMessage += HintMessage( true, hint.Prompt.Color );
	}
	
    if ( hint.Prompt.Value != "0" )
	{
        hintMessage += HintMessage( true, hint.Prompt.Value );
	}
	
	for ( var count = 0; count < hint.Prompt.ImpossibleSet.length; ++ count )
	{
	    var prompt = hint.Prompt.ImpossibleSet[ count ];
        hintMessage += HintMessage( false, prompt );
    }
    
    if ( hintMessage == "" )
    {
        hintMessage = "沒有提示";
    }
    $( handCard ).attr( "title", hintMessage );
    
    handCard.find( ".PlayCardTag:first" ).attr( "data-CardIndex", cardIndex )
        .click( function() { RequestPlay( this.getAttribute( "data-CardIndex" ) ); } );
    handCard.find( ".DiscardCardTag:first" ).attr( "data-CardIndex", cardIndex )
        .click( function() { RequestDiscard( this.getAttribute( "data-CardIndex" ) ); } );
    handCard.tooltip();
	
	return handCard;
}

function RemoveCard( cardIndex )
{
	$( "#Card_" + cardIndex ).remove();
}

function CreateFaceUpCard( card )
{
    var buttonColor = "";
    var glyphIcon = "";
    switch ( card.Color )
    {
        case 10:
            buttonColor = "btn-info";
            glyphIcon = "glyphicon-asterisk";
            break;
            
        case 20:
            buttonColor = "btn-success";
            glyphIcon = "glyphicon-plus";
            break;
            
        case 30:
            buttonColor = "btn-warning";
            glyphIcon = "glyphicon-stop";
            break;
            
        case 40:
            buttonColor = "btn-danger";
            glyphIcon = "glyphicon-heart";
            break;
            
        case 50:
            buttonColor = "btn-default";
            glyphIcon = "glyphicon-remove";
            break;
    }
    var cardButton = $( "#TemplateFaceUpCard" ).clone();
    cardButton.removeClass( "Hided" ).removeClass( "btn-primary" ).addClass( buttonColor )
		.attr( "id", "Card_" + card.Index );
    cardButton.find( "span:first" ).addClass( glyphIcon );
    cardButton.find( "span:last" ).text( card.Value );
    
	return cardButton;
}

function HintMessage( sure, value )
{
    return ( sure ) ? " 這張牌是 " + GetPromptName( value ) + " " : " 這張牌不是 " + GetPromptName( value ) + " ";
}

function GetColorName( value )
{
	switch ( value )
	{
		case 1: return "藍色";
		case 2: return "綠色";
		case 3: return "黃色";
		case 4: return "紅色";
		case 5: return "白色";
		default: return "";
	}
}

function GetPromptName( value )
{
	switch ( value )
	{
		case 1: return "1";
		case 2: return "2";
		case 3: return "3";
		case 4: return "4";
		case 5: return "5";
		case 10: return "藍色";
		case 20: return "綠色";
		case 30: return "黃色";
		case 40: return "紅色";
		case 50: return "白色";
		default: return "";
	}
}

function RequestPrompt( nickname, prompt )
{
    var payload = 
    {
        nickname : nickname,
        PromptInformation : prompt
    };
    
    SendCommand( "PromptCard", payload );
}

function ResponsePrompt( payload )
{
	if ( payload.Result != "0" )
	{
		alert( "ResponsePrompt Error" );
		return;
	}
	
	UpdateToken( payload.Token );
	
	var nickname = payload.Nickname;
	if ( IsMe( nickname ) )
	{
		UpdateFaceDownCards( nickname, payload.Cards );
	}
	else
	{
		UpdateFaceUpCards( nickname, payload.Cards );
	}
}

function RequestPlay( cardIndex )
{
    var payload = 
    {
        CardIndex : cardIndex
    };
    
    SendCommand( "PlayCard", payload );
}

function ResponsePlay( payload )
{
	if ( payload.Result != "0" && payload.Result != "3" )
	{
		alert( "ResponsePlay Error" );
		return;
	}
	
	UpdateToken( payload.Token );
    UpdateDrawPile( payload.DrawPileCount );
	
	UpdateNewCard( payload.Nickname, payload.OldCard, payload.NewCard );
	
	if ( payload.Result == "0" ) // Success
	{
		UpdatePlayedCard( payload.OldCard );
	}
	else // FailNoSlot
	{
		UpdateDiscardedCard( payload.OldCard );
	}
}

function RequestDiscard( cardIndex )
{
    var payload = 
    {
        CardIndex : cardIndex
    };
    
    SendCommand( "DiscardCard", payload );
}

function ResponseDiscard( payload )
{
	if ( payload.Result != "0" )
	{
		alert( "ResponseDiscard Error" );
		return;
	}
	
	UpdateToken( payload.Token );
    UpdateDrawPile( payload.DrawPileCount );
	
	UpdateNewCard( payload.Nickname, payload.OldCard, payload.NewCard );
	
	UpdateDiscardedCard( payload.OldCard );
}

function UpdateNewCard( nickname, oldCard, newCard )
{
	RemoveCard( oldCard.Index );
	
	var playerNode = $( "#Player_" + nickname );
	if ( IsMe( nickname ) )
	{
		var handCard = CreateFaceDownCard( newCard, playerNode );
		playerNode.removeClass( "Hided" ).append( handCard );
	}
	else
	{
		var cardButton = CreateFaceUpCard( newCard );
		playerNode.find( "div:first" ).append( cardButton );
	}
}

function UpdatePlayedCard( card )
{
    var cardButton = CreateFaceUpCard( card );
	cardButton.addClass( "disabled" );
	$( "#PlayedCards" + card.Color ).append( cardButton );
}

function UpdateDiscardedCard( card )
{
    var cardButton = CreateFaceUpCard( card );
	cardButton.addClass( "disabled" );
	$( "#DiscardedCards" + card.Color ).append( cardButton );
}