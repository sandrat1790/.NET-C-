using Sabio.Models.Domain.Friends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Friends
{
    internal class FriendV3AddRequest
    {
        List<FriendV2AddRequest> Friends { get; set; }  
        
        List<Skill> Skills { get; set; }
    }
}
