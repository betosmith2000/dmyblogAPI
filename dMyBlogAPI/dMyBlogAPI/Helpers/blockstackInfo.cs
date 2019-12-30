using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dMyBlogAPI.Helpers
{
    public class Profile
    {
        public Dictionary<string,string> apps { get; set; }
        public string name { get; set; }
    }

    public class blockstackInfo
    {
        public string owner_address { get; set; }
        public Profile profile { get; set; }
        public string public_key { get; set; }
        public List<string> verifications { get; set; }
    }

    public class BlockstackRootObject
    {
        public blockstackInfo account { get; set; }
    }
}
