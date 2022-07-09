using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subfloor.Dtos
{
    public class Profile
    {
        public Profile() { }

        public Profile(Guid identityId)
        {
            IdentityId = identityId;
            Name = String.Empty;
            ClientProfiles = new List<ClientProfile>();
        }

        public Guid PersonId { get; set; }
        public Guid IdentityId { get; set; }
        public string Name { get; set; }
        public List<ClientProfile> ClientProfiles { get; set; }

        public class ClientProfile
        {
            public Guid ClientId { get; set; }      //which is in authorization-dev.clientauthorization
            public string ClientName { get; set; }
            public List<Role> Roles { get; set; }
        }
    }
}
