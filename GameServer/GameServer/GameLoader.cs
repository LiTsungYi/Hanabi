using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using GameCore;
using GameCore.Types;

namespace GameServer
{
    /// <summary>
    /// 遊戲載入器
    /// </summary>
    public class GameLoader
    {
        /// <summary>
        /// 從設定檔建立遊戲
        /// </summary>
        /// <returns>遊戲清單</returns>
        public static Dictionary<GameNameType, IGame> CreateGames()
        {
            Dictionary<GameNameType, IGame> games = new Dictionary<GameNameType, IGame>();
            string[] gameNames = ConfigurationManager.AppSettings.AllKeys
                .Where( key => key.StartsWith( "Games_" ) )
                .Select( key => ConfigurationManager.AppSettings[ key ] )
                .ToArray();
            foreach ( var gameName in gameNames )
            {
                try
                {
                    Assembly assembly = Assembly.Load( gameName );
                    string fullname = string.Format( "{0}.Game", gameName );
                    Type type = assembly.GetType( fullname );
                    if ( type == null )
                    {
                        continue;
                    }

                    GameNameType key = new GameNameType( gameName );
                    ConstructorInfo ctor = type.GetConstructor( new[] { typeof( GameNameType ) } );
                    IGame game = ( IGame ) ctor.Invoke( new object[] { key } );
                    games.Add( key, game );
                }
                catch ( Exception e )
                {
                    string s = e.Message;
                }
            }

            return games;
        }
    }
}
