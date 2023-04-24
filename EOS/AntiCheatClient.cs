using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GGD_Hack.EOS
{
    public struct BeginSessionOptions // TypeDefIndex: 748
    {
        public int ApiVersion;
        //ProductUserId
        public IntPtr LocalUserId;
        public AntiCheatClientMode Mode;

        public BeginSessionOptions(int ApiVersion, IntPtr LocalUserId, AntiCheatClientMode Mode) : this()
        {
            this.ApiVersion = ApiVersion;
            this.LocalUserId = LocalUserId;
            this.Mode = Mode;
        }

        public static implicit operator BeginSessionOptions(JMDPPJDHNKC v)
        {
            return new BeginSessionOptions(
                    v.FPGKDHKEIOA,
                    v.EAJHLEKONDM,
                    (AntiCheatClientMode)v.KJBEAIPBDDK
                );
        }

        public override string ToString()
        {
            return string.Format("ApiVersion:{0}\nLocalUserId:{1}\nAntiCheatClientMode:{2}",
                ApiVersion,
                LocalUserId,
                Mode.ToString()
                );
        }
    }

    public enum AntiCheatClientMode // TypeDefIndex: 749
    {
        Invalid = 0,
        ClientServer = 1,
        PeerToPeer = 2
    }
}