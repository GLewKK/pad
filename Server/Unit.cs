using MessageChannel;
using MessageChannel.Models;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using static Server.Program;

namespace Server
{
    public class Unit
    {
        private static int count = 1;
        public static byte[] Execute(bool result)
        {
            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();

            binFormatter.Serialize(mStream, ActualUsers);

            var byteArr = mStream.ToArray();

            if (!result)
            {
                return byteArr;
            }
            return default;
        }

        public static byte[] Execute(string result)
        {
            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();

            if (!string.IsNullOrEmpty(result))
            {
                using (var context = new MessageChannelContext())
                {
                    var entityToUpdate = context.Users.Where(x => x.ChatOrder == count).ToList();
                    
                    foreach (var item in entityToUpdate)
                    {
                        item.ChatOrder = 0;
                    }

                    context.Users.UpdateRange(entityToUpdate);
                    context.SaveChanges();

                    var model = context.Users.FirstOrDefault(x => x.UserName == result);
                    if (model != null)
                    {
                        model.ChatOrder = count;
                        ActualUsers.Add(model);

                        context.Users.Update(model);
                    }
                    else
                    {
                        var entity = new User
                        {
                            UserName = result,
                            ChatOrder = count
                        };

                        context.Users.Add(entity);
                        context.SaveChanges();
                        count++;

                        ActualUsers.Add(entity);
                    }
                }

                binFormatter.Serialize(mStream, true);

                return mStream.ToArray();
            }
            binFormatter.Serialize(mStream, false);

            return mStream.ToArray();
        }
    }
}
