using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BankDeposit.Model.SqlBank;
using BankDeposit.Model.Helper;
using Microsoft.EntityFrameworkCore;

namespace BankDeposit.Data
{
    /// <summary>
    /// 访问储户表Depositors，对Depositors表进行操作
    /// </summary>
    public class AccessIdcard
    {
        #region 查询储户 （注册的,绑定卡号的）
        /// <summary>
        /// 查询储户 （注册的,绑定卡号的）根据储户账号查询
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public Idcard QueryData(int ICid)
        {
            Idcard idCard  = new Idcard();
            using (var dbContext = new bankContext())
            {
                return idCard = dbContext.Idcard.FirstOrDefault(a => a.Icid == ICid);
            }
        }
        #endregion

        }
    }
