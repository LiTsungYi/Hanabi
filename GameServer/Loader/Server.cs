using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Fleck;
using GameCore;
using GameCore.Types;

namespace GameServer
{
    public class Server
    {
        public readonly string ServerIp;
        public readonly int ServerPort;

        private Dictionary<Guid, IPlayer> allSockets = new Dictionary<Guid, IPlayer>();
        private Dictionary<GameNameType, IGame> games = new Dictionary<GameNameType, IGame>();

        public Server()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.None );
            this.ServerIp = config.AppSettings.Settings[ "IP" ].Value;
            this.ServerPort = int.Parse( config.AppSettings.Settings[ "Port" ].Value );
        }

        public void Launch( Dictionary<GameNameType, IGame> games )
        {
            this.games = games;

            string serverAddress = string.Format( "ws://{0}:{1}", this.ServerIp, this.ServerPort );

            FleckLog.Level = LogLevel.Debug;
            var server = new WebSocketServer( serverAddress );
            server.SupportedSubProtocols = new[] { "Hanabi" };

            server.Start( socket =>
            {
                socket.OnOpen = () =>
                {
                    IGame game = GetGame( socket );
                    if ( game == null )
                    {
                        return;
                    }
                    IPlayer player = game.CreatePlayer( socket );
                    IWebSocketConnectionInfo connectionInfo = socket.ConnectionInfo;
                    allSockets.Add( connectionInfo.Id, player );
                };
                socket.OnClose = () =>
                {
                    IPlayer player = GetPlayer( socket );
                    if ( player != null )
                    {
                        IGame game = player.Game;
                        if ( game != null )
                        {
                            game.PlayerDisconnect( player );
                        }
                    }

                    IWebSocketConnectionInfo connectionInfo = socket.ConnectionInfo;
                    allSockets.Remove( connectionInfo.Id );
                };
                socket.OnMessage = ( message ) =>
                {
                    IPlayer player = null;
                    if ( !allSockets.TryGetValue( socket.ConnectionInfo.Id, out player ) )
                    {
                        return;
                    }

                    IWebSocketConnection client = player.Client;
                    string subProtocol = client.ConnectionInfo.SubProtocol;
                    IGame game = games.FirstOrDefault( pair => pair.Key.GameName == subProtocol ).Value;
                    if ( game != null )
                    {
                        game.DispatchRequest( player, message );
                    }
                };
            } );
        }

        private IGame GetGame( IWebSocketConnection client )
        {
            string subProtocol = client.ConnectionInfo.SubProtocol;
            return this.games.FirstOrDefault( pair => pair.Key.GameName == subProtocol ).Value;
        }

        private IPlayer GetPlayer( IWebSocketConnection client )
        {
            IPlayer player = null;
            this.allSockets.TryGetValue( client.ConnectionInfo.Id, out player );
            return player;
        }
    }
}
