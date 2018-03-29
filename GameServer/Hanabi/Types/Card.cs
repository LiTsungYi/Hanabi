using System.Collections.Generic;
using GameCore.Types;
using Hanabi.Payloads;

namespace Hanabi.Types
{
    public enum CardValueType
    {
        Unknown,
        Value1,
        Value2,
        Value3,
        Value4,
        Value5,
    }

    public enum CardColorType
    {
        Unknown = 0,
        Blue = 10,
        Green = 20,
        Yellow = 30,
        Red = 40,
        White = 50,
    }

    public enum PromptInformation
    {
        Value1 = CardValueType.Value1,
        Value2 = CardValueType.Value2,
        Value3 = CardValueType.Value3,
        Value4 = CardValueType.Value4,
        Value5 = CardValueType.Value5,
        ColorBlue = CardColorType.Blue,
        ColorGreen = CardColorType.Green,
        ColorYellow = CardColorType.Yellow,
        ColorRed = CardColorType.Red,
        ColorWhite = CardColorType.White,
    }

    /// <summary>
    /// 卡片識別碼
    /// </summary>
    public sealed class CardIdType : IdType<int>
    {
        public CardIdType( int id )
            : base( id )
        {
        }

        public int Value
        {
            get
            {
                return this.IdTypeValue;
            }
        }
    }

    /// <summary>
    /// 卡片編號，排庫抽出的第幾張，與牌面的顏色、數字無關
    /// </summary>
    public sealed class CardIndexType : IdType<int>
    {
        public CardIndexType( int index )
            : base( index )
        {
        }

        public int Value
        {
            get
            {
                return this.IdTypeValue;
            }
        }
    }

    public class Card
    {
        public Card( CardIdType id, CardIndexType index, CardColorType color, CardValueType value )
        {
            this.Id = id;
            this.Index = index;
            this.Color = color;
            this.Value = value;
            this.Information = new CardInformation();
        }

        public CardIdType Id
        {
            get;
            private set;
        }

        public CardIndexType Index
        {
            get;
            private set;
        }

        public CardColorType Color
        {
            get;
            set;
        }

        public CardValueType Value
        {
            get;
            set;
        }

        public CardInformation Information
        {
            get;
            set;
        }

        public CardInfo Info
        {
            get
            {
                CardInfo info = new CardInfo();
                info.Index = Index.Value;
                info.Color = ( int ) Color;
                info.Value = ( int ) Value;
                info.Prompt = Information.PromptInfo;
                return info;
            }
        }
    }

    public class CardInformation
    {
        public CardInformation()
        {
            Color = CardColorType.Unknown;
            Value = CardValueType.Unknown;
            ImpossiblePrompt = new HashSet<PromptInformation>();
        }

        /// <summary>
        /// 已知牌的顏色
        /// </summary>
        public CardColorType Color
        {
            get;
            set;
        }

        /// <summary>
        /// 已知牌的數字
        /// </summary>
        public CardValueType Value
        {
            get;
            set;
        }

        /// <summary>
        /// 已知牌的資訊
        /// </summary>
        public HashSet<PromptInformation> ImpossiblePrompt
        {
            get;
            private set;
        }

        public CardPrompt PromptInfo
        {
            get
            {
                CardPrompt prompt = new CardPrompt();
                prompt.Color = ( int ) Color;
                prompt.Value = ( int ) Value;
                prompt.ImpossibleSet = new List<int>();
                foreach ( var promptInformation in ImpossiblePrompt )
                {
                    prompt.ImpossibleSet.Add( ( int ) promptInformation );
                }
                return prompt;
            }
        }

        /// <summary>
        /// 提示此牌的顏色
        /// </summary>
        /// <param name="promptColor">提示的顏色</param>
        /// <param name="realColor">牌的顏色</param>
        public void Prompt( CardColorType promptColor, CardColorType realColor )
        {
            if ( promptColor == realColor )
            {
                this.Color = realColor;
            }
            else if ( !ImpossiblePrompt.Contains( ( PromptInformation ) promptColor ) )
            {
                ImpossiblePrompt.Add( ( PromptInformation ) promptColor );
            }
        }

        /// <summary>
        /// 提示此牌的數字
        /// </summary>
        /// <param name="promptValue">提示的數字</param>
        /// <param name="realValue">牌的數字</param>
        public void Prompt( CardValueType promptValue, CardValueType realValue )
        {
            if ( promptValue == realValue )
            {
                this.Value = realValue;
            }
            else if ( !ImpossiblePrompt.Contains( ( PromptInformation ) promptValue ) )
            {
                ImpossiblePrompt.Add( ( PromptInformation ) promptValue );
            }
        }
    }
}
