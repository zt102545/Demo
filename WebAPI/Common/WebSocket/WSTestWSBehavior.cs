﻿using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Common.WebSocket;

namespace WebAPI.Common
{
    public class WSTestWSBehavior : BasicWebSocketBehavior
    {
        public static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, WSTestWSBehavior>> GroupSessions = new ConcurrentDictionary<string, ConcurrentDictionary<string, WSTestWSBehavior>>();

        protected override void OnClose(CloseEventArgs e)
        {
            if (GroupSessions != null && GroupSessions.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, ConcurrentDictionary<string, WSTestWSBehavior>> item in GroupSessions)
                {
                    RemoveFromGroup(item.Key, Id);
                }
            }
            base.OnClose(e);
        }

        public static void RemoveFromGroup(string groupId, string sessionId)
        {
            GroupSessions.TryGetValue(groupId, out ConcurrentDictionary<string, WSTestWSBehavior> dic);
            if (dic != null)
            {
                dic.TryRemove(sessionId, out WSTestWSBehavior session);
            }
        }

        /// <summary>
        /// 创建链接
        /// </summary>
        public override void OnOpen()
        {
            base.OnOpen();

            //monitorpannel?usertoken={usertoken}&packtype={packtype}&version={version}&lang={lang}&deviceid={deviceid}&
            string usertoken = Context.Request.Query["usertoken"];
            if (usertoken == null)
            {
                return;
            }

            string key = "testkey";
            if (!string.IsNullOrEmpty(key))
            {
                WSTestSend.sessionKey = key;
                JoinGroup(key, this);
            }
        }

        /// <summary>
        /// 创建组
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="session"></param>
        public static void JoinGroup(string groupId, WSTestWSBehavior session)
        {
            ConcurrentDictionary<string, WSTestWSBehavior> dic = GroupSessions.GetOrAdd(groupId, m => new ConcurrentDictionary<string, WSTestWSBehavior>());
            dic.AddOrUpdate(session.Id, session, (k, v) => session);
        }

        /// <summary>
        /// 接受客户端消息
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
            var data = JsonConvert.DeserializeObject<AcceptModel>(e.Data);
        }
    }

    public class AcceptModel
    {
        public long time { get; set; }
    }
}
