using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.IO;

namespace Player
{
    public class ImportExport
    {
        /// <summary>
        /// import /account/{name}.json to Account.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Account importAccount(string name)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            string readJson;
            try
            {
                readJson = File.ReadAllText(Path.Combine("profile", name + ".json"));
            }
            catch (Exception ex)
            {
                return null;
            }
            return json.Deserialize<Account>(readJson);
        }

        /// <summary>
        /// export Account to json at /account/{name}.json
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public int exportAccount(Account account)
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            string writeJson = json.Serialize(account);
            try
            {
                if (account.mID == null)
                {
                    File.WriteAllText(Path.Combine(makeFolder("profile"), "default.json"), writeJson);//account 에서 profile로 바꿈
                }
                else
                {
                    File.WriteAllText(Path.Combine(makeFolder("profile"), account.mID + ".json"), writeJson);
                }

            }
            catch (Exception ex)
            {
                return -1;
            }

            return 0;
        }


        public string makeFolder(string folderName)
        {
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), folderName));
            return Path.Combine(Directory.GetCurrentDirectory(), folderName);
        }
    }
}
