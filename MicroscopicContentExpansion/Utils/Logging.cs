using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem.LogThreads.Common;
using UnityEngine;

namespace MicroscopicContentExpansion.Utils;
public static class Logging
{
    public static void CombatLog(string msg)
    {
        CombatLogMessage message;
        message = new CombatLogMessage(msg, Color.black, PrefixIcon.None, null, false);
        var messageLog = LogThreadService.Instance.m_Logs[LogChannelType.Common].First(x => x is MessageLogThread);
        messageLog.AddMessage(message);
    }
}
