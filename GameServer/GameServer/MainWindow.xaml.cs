using System.Collections.Generic;
using System.Windows;
using GameCore;
using GameCore.Types;

namespace GameServer
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 網路類別
        /// </summary>
        private Server server = null;

        /// <summary>
        /// 主視窗
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            Dictionary<GameNameType, IGame> games = GameLoader.CreateGames();

            this.server = new Server();
            this.server.Launch( games );

            this.Title = string.Format( "桌遊伺服器 - ws://{0}:{1}", this.server.ServerIp, this.server.ServerPort );
        }
    }
}
