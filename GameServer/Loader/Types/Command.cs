namespace GameCore.Types
{
    /// <summary>
    /// 溝通用的封包
    /// </summary>
    public class Command
    {
        /// <summary>
        /// 封包的命令
        /// </summary>
        public string Action
        {
            get;
            set;
        }

        /// <summary>
        /// 封包的資料
        /// </summary>
        public string Payload
        {
            get;
            set;
        }
    }
}
