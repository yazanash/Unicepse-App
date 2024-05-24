using PlatinumGym.Core.Models.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Core.Exceptions
{

    public class PlayerConflictException : Exception
    {
        public Player ExistingPlayer { get; }
        public Player IncomingPlayer { get; }
        public PlayerConflictException(Player existingPlayer, Player incomingPlayer)
        {
            ExistingPlayer = existingPlayer;
            IncomingPlayer = incomingPlayer;
        }

        public PlayerConflictException(Player existingPlayer, Player incomingPlayer,string? message) : base(message)
        {
            ExistingPlayer = existingPlayer;
            IncomingPlayer = incomingPlayer;
        }
        public PlayerConflictException( string? message) : base(message)
        {
            
        }

        public PlayerConflictException(Player existingPlayer, Player incomingPlayer,string? message, Exception? innerException) : base(message, innerException)
        {
            ExistingPlayer = existingPlayer;
            IncomingPlayer = incomingPlayer;
        }

        protected PlayerConflictException(Player existingPlayer, Player incomingPlayer,SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ExistingPlayer = existingPlayer;
            IncomingPlayer = incomingPlayer;
        }

        
    }
}
