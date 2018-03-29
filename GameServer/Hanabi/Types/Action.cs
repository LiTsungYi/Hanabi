namespace Hanabi.Types
{
    /// <summary>
    /// 命令的類型
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 進入廳館
        /// </summary>
        EnterGame,

        /// <summary>
        /// 離開廳館
        /// </summary>
        ExitGame,

        /// <summary>
        /// 取得牌桌列表
        /// </summary>
        GetRoomList,

        /// <summary>
        /// 進入牌桌
        /// </summary>
        JoinRoom,

        /// <summary>
        /// 離開牌桌
        /// </summary>
        QuitRoom,

        /// <summary>
        /// 聊天訊息
        /// </summary>
        Message,

        /// <summary>
        /// 玩家準備好了
        /// </summary>
        Ready,

        /// <summary>
        /// 提示手牌資訊
        /// </summary>
        PromptCard,

        /// <summary>
        /// 出牌
        /// </summary>
        PlayCard,

        /// <summary>
        /// 棄牌
        /// </summary>
        DiscardCard,

        /// <summary>
        /// 通知牌局資訊
        /// </summary>
        NotifyBoard,

        /// <summary>
        /// 通知行動玩家
        /// </summary>
        NotifyTurn,

        /// <summary>
        /// 通知遊戲結束
        /// </summary>
        NotifyRound,
    }
}
