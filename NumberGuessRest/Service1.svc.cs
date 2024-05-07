using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.SessionState;

namespace NumberGuessRest
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private static int sNumber;

        public int SecretNumber(string lower, string upper)
        {
            int lowerNum = Convert.ToInt32(lower);
            int upperNum = Convert.ToInt32(upper);

            //generates a random number that will be stored as a secret number
            DateTime currentDate = DateTime.Now;
            int seed = (int)currentDate.Ticks;
            Random random = new Random(seed);
            int sNumber = random.Next(lowerNum, upperNum);
            return sNumber;
        }

        public string CheckNumber(string userNum, string SecretNum)
        {
            //checks if the user number is equal to the secret number, if so the function returns correct

            int userNumber = Convert.ToInt32(userNum);
            int secretNumber = Convert.ToInt32(SecretNum);

            if (userNumber == secretNumber)
            {
                return "correct";
            }
            else
            {
                if (userNumber > secretNumber)
                {
                    return "too big";
                }
                else
                {
                    return "too small";
                }
            }
        }
    }
}
