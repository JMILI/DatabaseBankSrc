using BankDeposit.Data;
using BankDeposit.Model.SqlBank;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using BankDeposit.Model.Helper;
using System.Linq;
using System.Text;

namespace BankDeposit.Service
{
    public class IdcardService
    {
        #region 实例化一些工具对象
        public static AccessIdcard accessIdcard = new AccessIdcard();
        #endregion


        #region 查询身份信息
        /// <summary>
        /// 查询身份信息
        /// </summary>
        /// <param name="ICid">传入身份证号ICid</param>
        /// <returns></returns>
        public Idcard QueryData(int ICid)
        {
            return accessIdcard.QueryData(ICid);
        }
        #endregion

       
    }
}
