using HBL_MLDV_APP.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace HBL_MLDV_APP.Repository
{
    public class Cando
    {
        private async Task<bool> CheckRights(int value, int bits)
        {
            return  (Math.Round((decimal)(value - (bits / 2)) / bits, 0) % 2) == 1 ? true : false;
        }

        public async Task<bool> CandoResult(UserAccountDetails rights, int bit)
        {
            if (rights != null)
            {
                var score = rights.CanAdd + rights.CanEdit  + rights.CanView + rights.CanDel;

                return await CheckRights((int)score, bit);

            }
            else
            {
                return false;
            }

            //BITS TO CHECK RIGHTS ON SPECIFIC ACTION
            //CREATE    EDIT    REMOVE  SELECT
            //    2      4       8       1

        }

    }
}