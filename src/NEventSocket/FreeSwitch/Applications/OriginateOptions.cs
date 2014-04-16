﻿namespace NEventSocket.FreeSwitch.Applications
{
    using System.Collections.Generic;
    using System.Text;

    using NEventSocket.Util;

    /// <summary>
    /// Represents options that may be used with the Originate api command
    /// </summary>
    /// <remarks>
    /// See https://wiki.freeswitch.org/wiki/Channel_Variables#Originate_related_variables
    /// </remarks>
    public class OriginateOptions
    {
        private readonly IDictionary<string, string> parameters = new Dictionary<string, string>(); 

        public OriginateOptions()
        {
            ChannelVariables = new Dictionary<string, string>();
        }

        /// <summary>
        /// Optionally set the UUID of the outbound leg before originating the call.
        /// </summary>
        /// <remarks>
        /// NEventSocket will set the UUID if it is not set, so that we can catch events of interest on the channel.
        /// </remarks>
        public string UUID
        {
            get
            {
                return parameters.GetValueOrDefault("origination_uuid");
            }

            set
            {
                parameters["origination_uuid"] = value;
            }
        }

        /// <summary>
        /// Sets the outbound callerid name
        /// </summary>
        /// <remarks>
        /// See https://wiki.freeswitch.org/wiki/Cid
        /// </remarks>
        public string CallerIdName { set { parameters["origination_caller_id_name"] = value; } }

        /// <summary>
        /// Sets the outbound callerid number.
        /// </summary>
        /// <remarks>
        /// See https://wiki.freeswitch.org/wiki/Cid
        /// </remarks>
        public string CallerIdNumber { set { parameters["origination_caller_id_number"] = value; } }


        /// <summary>
        /// Number of retries before giving up on originating a call (default is 0).
        /// </summary>
        public int Retries { set { parameters["originate_retries"] = value.ToString(); } }

        /// <summary>
        /// This will set how long FreeSWITCH is going to wait between sending invite messages to the receiving gateway
        /// </summary>
        public int RetrySleepMs { set { parameters["originate_retry_sleep_ms"] = value.ToString(); } }

        /// <summary>
        /// The maximum number of seconds to wait for an answer from a remote endpoint.
        /// </summary>
        public int TimeoutSeconds { set { parameters["originate_timeout"] = value.ToString(); } }
        
        /// <summary>
        /// Executes code on successful origination. Use the '{app} {arg}' format to execute in the origination thread or use '{app}::{arg}' to execute asynchronously. 
        /// Successful origination means the remote server responds, NOT when the call is actually answered. 
        /// </summary>
        public string ExecuteOnOriginate { set { parameters["execute_on_originate"] = value; } }

        /// <summary>
        /// Whether this call should hang up after completing a bridge to another leg
        /// </summary>
        public bool HangupAfterBridge { set { ChannelVariables["hangup_after_bridge"] = value.ToLowerBooleanString(); } }

        /// <summary>
        /// Whether the originate command should complete on receiving a RingReady event.
        /// Can be used to route the call to an outbound socket to recive the CHANNEL_ANSWER event.
        /// </summary>
        /// <remarks>
        /// See http://blog.godson.in/2010/12/use-of-returnringready-originate.html
        /// </remarks>
        public bool ReturnRingReady
        {
            get
            {
                bool returnRingReady;
                return bool.TryParse(this.parameters.GetValueOrDefault("return_ring_ready"), out returnRingReady) && returnRingReady;
            }

            set
            {
                parameters["return_ring_ready"] = value.ToLowerBooleanString();
            }
        }

        /// <summary>
        /// Whether Early Media responses should be ignored when determining whether an originate or bridge has completed successfully.
        /// </summary>
        public bool IgnoreEarlyMedia { set { parameters["ignore_early_media"] = value.ToLowerBooleanString(); } }

        /// <summary>
        /// When bridging a call, No media mode is an SDP Passthrough feature that permits two endpoints that can see each other (no funky NAT's) to connect their media sessions directly while FreeSWITCH maintains control of the SIP signaling.
        /// </summary>
        /// <remarks>
        /// Before executing the bridge action you must set the "bypass_media" flag to true. bypass_media must only be set on the A leg of a call.
        /// https://wiki.freeswitch.org/wiki/Misc._Dialplan_Tools_bridge
        /// https://wiki.freeswitch.org/wiki/Bypass_Media
        /// </remarks>
        public bool BypassMedia { set { parameters["bypass_media"] = value.ToLowerBooleanString(); } }

        /// <summary>
        /// Container for any Channel Variables to be set before executing the origination
        /// </summary>
        public IDictionary<string, string> ChannelVariables { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("{");

            sb.Append(parameters.ToOriginateString());
            sb.Append(ChannelVariables.ToOriginateString());
            
            if (sb.Length > 1)
                sb.Remove(sb.Length - 1, 1);

            sb.Append("}");

            return sb.ToString();
        }
    }
}