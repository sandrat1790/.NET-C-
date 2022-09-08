using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Friends
{
    public class FriendV2UpdateRequest : FriendV2AddRequest , IModelIdentifier
    {
        public int Id { get; set; }
    }
}
