using MessageChannel.Abstractions;
using MessageChannel.Models;
using System.Linq;

namespace MessageChannel.Implementations
{
    public class ContextService : IContextService
    {
        public void Add(string name)
        {
            using(var dbContext = new MessageChannelContext())
            {
                if (!dbContext.Users.Any(x => x.UserName.Contains(name)))
                {
                    var model = new User
                    {
                        UserName = name
                    };

                    dbContext.Add(model);
                }
            }
        }
    }
}
