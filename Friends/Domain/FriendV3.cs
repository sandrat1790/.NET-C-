using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Friends
{
    public class FriendV3 : FriendBase
    {
        public Image PrimaryImage { get; set; }
        public List<Skill> Skills { get; set; }

    }
}
